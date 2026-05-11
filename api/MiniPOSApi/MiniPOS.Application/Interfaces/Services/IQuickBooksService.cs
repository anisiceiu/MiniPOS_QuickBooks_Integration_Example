using MiniPOS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniPOS.Application.Interfaces.Services
{
    public interface IQuickBooksService
    {
        Task<string> CreateInvoiceAsync(Sale sales);
    }
}
