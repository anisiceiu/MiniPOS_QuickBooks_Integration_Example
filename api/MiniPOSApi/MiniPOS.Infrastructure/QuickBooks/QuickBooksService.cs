using Microsoft.Extensions.Options;
using MiniPOS.Application.Interfaces.Services;
using MiniPOS.Domain.Entities;
using MiniPOS.Infrastructure.QuickBooks.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniPOS.Infrastructure.QuickBooks
{
    public class QuickBooksService : IQuickBooksService
    {
        private readonly QuickBooksHttpClient _client;
        private readonly QuickBooksOptions _options;
        private readonly ITokenRepository _tokenRepo;
        private readonly IQuickBooksAuthService _authService;

        public QuickBooksService(
            QuickBooksHttpClient client,
            IOptions<QuickBooksOptions> options,
            ITokenRepository tokenRepo,
            IQuickBooksAuthService authService)
        {
            _client = client;
            _options = options.Value;
            _tokenRepo = tokenRepo;
            _authService = authService;
        }

        public async Task<string> CreateInvoiceAsync(Sale sales)
        {
            var token = await _tokenRepo.GetLatestTokenAsync();

            if (token.AccessTokenExpiresAt <= DateTime.UtcNow)
            {
                // refresh automatically
                token = await RefreshToken(token);
            }

            var invoice = MapToInvoice(sales);

            var url = $"{_options.BaseUrl}/{token.RealmId}/invoice";

            var json = JsonConvert.SerializeObject(invoice);

            var result = await _client.PostAsync(url, json, token.AccessToken);

            return result;
        }

        private object MapToInvoice(Sale sales)
        {
            return new
            {
                CustomerRef = new { value = sales.CustomerId },
                Line = sales.SaleItems.Select(x => new
                {
                    Amount = x.LineTotal,
                    DetailType = "SalesItemLineDetail",
                    SalesItemLineDetail = new
                    {
                        Qty = x.Quantity,
                        UnitPrice = x.UnitPrice,
                        ItemRef = new { value = x.ProductId }
                    }
                })
            };
        }

        private async Task<QuickBooksToken> RefreshToken(QuickBooksToken token)
        {
            if (token == null)
                throw new Exception("QuickBooks token not found.");

            if (string.IsNullOrWhiteSpace(token.RefreshToken))
                throw new Exception("QuickBooks refresh token is missing.");

            // Request new token from QuickBooks
            var refreshedToken =
                await _authService.RefreshTokenAsync(token.RefreshToken);

            if (refreshedToken == null)
                throw new Exception("Failed to refresh QuickBooks token.");

            // Update existing token entity
            token.AccessToken = refreshedToken.AccessToken;

            // QuickBooks may rotate refresh token
            if (!string.IsNullOrWhiteSpace(refreshedToken.RefreshToken))
            {
                token.RefreshToken = refreshedToken.RefreshToken;
            }

            token.AccessTokenExpiresAt =
                DateTime.UtcNow.AddSeconds(refreshedToken.ExpiresIn);

            // Save updated token in DB
            await _tokenRepo.UpdateAsync(token);

            return token;
        }
    }
}
