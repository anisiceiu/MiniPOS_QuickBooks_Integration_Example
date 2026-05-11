using Microsoft.Extensions.Options;
using MiniPOS.Infrastructure.QuickBooks.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MiniPOS.Infrastructure.QuickBooks
{
    public class QuickBooksAuthService : IQuickBooksAuthService
    {
        private readonly HttpClient _client;
        private readonly QuickBooksOptions _options;

        public QuickBooksAuthService(HttpClient client, IOptions<QuickBooksOptions> options)
        {
            _client = client;
            _options = options.Value;
        }

        public async Task<QuickBooksTokenResponse> ExchangeCodeAsync(string code)
        {
            return await RequestToken(new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["code"] = code,
                ["redirect_uri"] = _options.RedirectUri
            });
        }

        public async Task<QuickBooksTokenResponse> RefreshTokenAsync(string refreshToken)
        {
            return await RequestToken(new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["refresh_token"] = refreshToken
            });
        }

        private async Task<QuickBooksTokenResponse> RequestToken(Dictionary<string, string> form)
        {
            var authHeader = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{_options.ClientId}:{_options.ClientSecret}")
            );

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", authHeader);

            var response = await _client.PostAsync(
                _options.TokenUrl,
                new FormUrlEncodedContent(form)
            );

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<QuickBooksTokenResponse>(json);
        }
    }
}
