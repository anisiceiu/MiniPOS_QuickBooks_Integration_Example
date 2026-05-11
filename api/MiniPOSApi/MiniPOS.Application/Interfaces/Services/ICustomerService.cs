using MiniPOS.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniPOS.Application.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<CustomerDto> GetCustomerByIdAsync(int id);
        Task<List<CustomerDto>> GetAllCustomersAsync();
        Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto dto);
        Task<CustomerDto> UpdateCustomerAsync(int id, UpdateCustomerDto dto);
        Task DeleteCustomerAsync(int id);
    }
}