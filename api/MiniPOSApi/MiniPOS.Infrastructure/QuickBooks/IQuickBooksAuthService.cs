using MiniPOS.Infrastructure.QuickBooks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniPOS.Infrastructure.QuickBooks
{
    public interface IQuickBooksAuthService
    {
        Task<QuickBooksTokenResponse> ExchangeCodeAsync(string code);
        Task<QuickBooksTokenResponse> RefreshTokenAsync(string refreshToken);
    }
}
