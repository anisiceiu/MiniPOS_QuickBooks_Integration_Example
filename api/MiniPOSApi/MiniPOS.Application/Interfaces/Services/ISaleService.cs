using MiniPOS.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniPOS.Application.Interfaces.Services
{
    public interface ISaleService
    {
        Task<SaleDto> GetSaleByIdAsync(int id);
        Task<List<SaleDto>> GetAllSalesAsync();
        Task<SaleDto> CreateSaleAsync(CreateSaleDto dto);
        Task<SaleDto> UpdateSaleAsync(int id, UpdateSaleDto dto);
        Task DeleteSaleAsync(int id);
    }
}