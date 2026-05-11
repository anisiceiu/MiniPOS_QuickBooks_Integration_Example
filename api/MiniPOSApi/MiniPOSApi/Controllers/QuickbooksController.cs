using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniPOS.Application.QuickBooks;
using MiniPOS.Domain.Entities;
using MiniPOS.Infrastructure.QuickBooks;

namespace MiniPOSApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuickbooksController : ControllerBase
    {
        IConfiguration _config;
        QuickBooksAuthService _authService;
        ITokenRepository _tokenRepository;   
        public QuickbooksController(IConfiguration config, QuickBooksAuthService authService, ITokenRepository tokenRepository)
        {
            _config = config;
            _authService = authService;
            _tokenRepository = tokenRepository;
        }

        [HttpGet("connect")]
        public IActionResult Connect()
        {
            var clientId = _config["QuickBooks:ClientId"];

            var redirectUri =
                _config["QuickBooks:RedirectUri"];

            var url =
                "https://appcenter.intuit.com/connect/oauth2" +
                "?client_id=" + clientId +
                "&response_type=code" +
                "&scope=com.intuit.quickbooks.accounting" +
                "&redirect_uri=" + Uri.EscapeDataString(redirectUri) +
                "&state=123";

            return Redirect(url);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback(
    string code,
    string realmId)
        {
            var token =
                await ((IQuickBooksAuthService)_authService).ExchangeCodeAsync(code);

            var entity = new QuickBooksToken
            {
                AccessToken = token.access_token,
                RefreshToken = token.refresh_token,
                RealmId = realmId,
                AccessTokenExpiresAt =
                    DateTime.UtcNow.AddSeconds(token.expires_in)
            };

            await _tokenRepository.SaveAsync(entity);

            return Ok("QuickBooks connected successfully");
        }
    }
}
