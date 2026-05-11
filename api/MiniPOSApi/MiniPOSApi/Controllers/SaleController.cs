using Microsoft.AspNetCore.Mvc;
using MiniPOS.Application.DTOs;
using MiniPOS.Application.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniPOSApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SaleController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSales()
        {
            var sales = await _saleService.GetAllSalesAsync();
            return Ok(sales);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSaleById(int id)
        {
            var sale = await _saleService.GetSaleByIdAsync(id);
            return Ok(sale);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleDto dto)
        {
            var sale = await _saleService.CreateSaleAsync(dto);
            return CreatedAtAction(nameof(GetSaleById), new { id = sale.Id }, sale);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSale(int id, [FromBody] UpdateSaleDto dto)
        {
            var sale = await _saleService.UpdateSaleAsync(id, dto);
            return Ok(sale);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSale(int id)
        {
            await _saleService.DeleteSaleAsync(id);
            return NoContent();
        }
    }
}