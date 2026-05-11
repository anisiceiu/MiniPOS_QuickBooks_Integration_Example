
using MiniPOS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniPOS.Application.QuickBooks
{
    public interface IQuickBooksAuthService
    {
        Task<Application.DTOs.QuickBooksTokenResponse> ExchangeCodeAsync(string code);
        Task<Application.DTOs.QuickBooksTokenResponse> RefreshTokenAsync(string refreshToken);
    }
}
