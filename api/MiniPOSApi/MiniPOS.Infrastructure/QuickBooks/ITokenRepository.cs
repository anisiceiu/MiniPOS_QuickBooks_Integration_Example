using MiniPOS.Domain.Entities;
using MiniPOS.Infrastructure.QuickBooks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniPOS.Infrastructure.QuickBooks
{
    public interface ITokenRepository
    {
        Task<QuickBooksToken> GetLatestTokenAsync();
        Task SaveAsync(QuickBooksToken token);
        Task UpdateAsync(QuickBooksToken token);
    }
}
