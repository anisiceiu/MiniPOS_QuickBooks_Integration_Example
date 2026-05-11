using MiniPOS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniPOS.Application.QuickBooks
{
    public interface ITokenRepository
    {
        Task<QuickBooksToken> GetLatestTokenAsync();
        Task SaveAsync(QuickBooksToken token);
        Task UpdateAsync(QuickBooksToken token);
    }
}
