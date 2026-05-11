using MiniPOS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniPOS.Application.Interfaces
{
    public interface IQuickBooksCustomerService
    {
        Task<string> CreateCustomerAsync(Customer customer, string accessToken, string realmId);
        Task<string> UpdateCustomerAsync(Customer customer, string qbCustomerId, string accessToken, string realmId);
    }
}
