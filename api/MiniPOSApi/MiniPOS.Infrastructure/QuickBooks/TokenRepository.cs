using Microsoft.EntityFrameworkCore;
using MiniPOS.Domain.Entities;
using MiniPOS.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniPOS.Infrastructure.QuickBooks
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ApplicationDbContext _context;

        public TokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<QuickBooksToken?> GetLatestTokenAsync()
        {
            return await _context.QuickBooksTokens
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();
        }

        public async Task SaveAsync(QuickBooksToken token)
        {
     
            token.UpdatedAt = DateTime.UtcNow;

            await _context.QuickBooksTokens.AddAsync(token);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(QuickBooksToken token)
        {
            token.UpdatedAt = DateTime.UtcNow;

            _context.QuickBooksTokens.Update(token);

            await _context.SaveChangesAsync();
        }
    }
}
