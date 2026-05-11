using MiniPOS.Application.DTOs;
using MiniPOS.Application.Interfaces;
using MiniPOS.Application.Interfaces.Services;
using MiniPOS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniPOS.Application.Services
{
    public class SaleService : ISaleService
    {
        private readonly IApplicationDbContext _context;

        public SaleService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SaleDto> GetSaleByIdAsync(int id)
        {
            var sale = await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.SaleItems)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (sale == null) throw new KeyNotFoundException("Sale not found");
            return MapToDto(sale);
        }

        public async Task<List<SaleDto>> GetAllSalesAsync()
        {
            var sales = await _context.Sales.ToListAsync();
            return sales.Select(MapToDto).ToList();
        }

        public async Task<SaleDto> CreateSaleAsync(CreateSaleDto dto)
        {
            var subTotal = dto.SaleItems.Sum(si => si.UnitPrice * si.Quantity - si.DiscountAmount);
            var totalAmount = subTotal + dto.TaxAmount + dto.ShippingAmount - dto.DiscountAmount;

            var sale = new Sale
            {
                CustomerId = dto.CustomerId,
                SaleDate = dto.SaleDate,
                InvoiceNumber = dto.InvoiceNumber,
                DueDate = dto.DueDate,
                DiscountAmount = dto.DiscountAmount,
                TaxAmount = dto.TaxAmount,
                ShippingAmount = dto.ShippingAmount,
                SubTotal = subTotal,
                TotalAmount = totalAmount,
                Currency = dto.Currency,
                Notes = dto.Notes,
                PaymentStatus = dto.PaymentStatus
            };
            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            foreach (var itemDto in dto.SaleItems)
            {
                var lineTotal = itemDto.UnitPrice * itemDto.Quantity - itemDto.DiscountAmount + itemDto.TaxAmount;
                var item = new SaleItem
                {
                    SaleId = sale.Id,
                    ProductId = itemDto.ProductId,
                    Description = itemDto.Description,
                    UnitPrice = itemDto.UnitPrice,
                    Quantity = itemDto.Quantity,
                    DiscountAmount = itemDto.DiscountAmount,
                    TaxAmount = itemDto.TaxAmount,
                    LineTotal = lineTotal
                };
                _context.SaleItems.Add(item);
            }
            await _context.SaveChangesAsync();

            return await GetSaleByIdAsync(sale.Id);
        }

        public async Task<SaleDto> UpdateSaleAsync(int id, UpdateSaleDto dto)
        {
            var sale = await _context.Sales.FindAsync(id);
            if (sale == null) throw new KeyNotFoundException("Sale not found");

            var subTotal = dto.SaleItems.Sum(si => si.UnitPrice * si.Quantity - si.DiscountAmount);
            var totalAmount = subTotal + dto.TaxAmount + dto.ShippingAmount - dto.DiscountAmount;

            sale.CustomerId = dto.CustomerId;
            sale.SaleDate = dto.SaleDate;
            sale.InvoiceNumber = dto.InvoiceNumber;
            sale.DueDate = dto.DueDate;
            sale.DiscountAmount = dto.DiscountAmount;
            sale.TaxAmount = dto.TaxAmount;
            sale.ShippingAmount = dto.ShippingAmount;
            sale.SubTotal = subTotal;
            sale.TotalAmount = totalAmount;
            sale.Currency = dto.Currency;
            sale.Notes = dto.Notes;
            sale.PaymentStatus = dto.PaymentStatus;

            // Remove existing SaleItems
            var existingItems = _context.SaleItems.Where(si => si.SaleId == id);
            _context.SaleItems.RemoveRange(existingItems);

            // Add new SaleItems
            foreach (var itemDto in dto.SaleItems)
            {
                var lineTotal = itemDto.UnitPrice * itemDto.Quantity - itemDto.DiscountAmount + itemDto.TaxAmount;
                var item = new SaleItem
                {
                    SaleId = sale.Id,
                    ProductId = itemDto.ProductId,
                    Description = itemDto.Description,
                    UnitPrice = itemDto.UnitPrice,
                    Quantity = itemDto.Quantity,
                    DiscountAmount = itemDto.DiscountAmount,
                    TaxAmount = itemDto.TaxAmount,
                    LineTotal = lineTotal
                };
                _context.SaleItems.Add(item);
            }

            await _context.SaveChangesAsync();

            return await GetSaleByIdAsync(sale.Id);
        }

        public async Task DeleteSaleAsync(int id)
        {
            var sale = await _context.Sales.FindAsync(id);
            if (sale == null) throw new KeyNotFoundException("Sale not found");
            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();
        }

        private SaleDto MapToDto(Sale sale)
        {
            return new SaleDto
            {
                Id = sale.Id,
                CustomerId = sale.CustomerId,
                SaleDate = sale.SaleDate,
                InvoiceNumber = sale.InvoiceNumber,
                DueDate = sale.DueDate,
                DiscountAmount = sale.DiscountAmount,
                TaxAmount = sale.TaxAmount,
                ShippingAmount = sale.ShippingAmount,
                SubTotal = sale.SubTotal,
                TotalAmount = sale.TotalAmount,
                Currency = sale.Currency,
                Notes = sale.Notes,
                PaymentStatus = sale.PaymentStatus,
                SyncStatus = sale.SyncStatus,
                QuickBooksInvoiceId = sale.QuickBooksInvoiceId,
                LastSyncedAt = sale.LastSyncedAt,
                Customer = sale.Customer != null ? new CustomerDto
                {
                    Id = sale.Customer.Id,
                    Name = sale.Customer.Name,
                    Email = sale.Customer.Email,
                    Phone = sale.Customer.Phone,
                    QuickBooksCustomerId = sale.Customer.QuickBooksCustomerId,
                    CreatedAt = sale.Customer.CreatedAt
                } : null,
                SaleItems = sale.SaleItems.Select(si => new SaleItemDto
                {
                    Id = si.Id,
                    ProductId = si.ProductId,
                    Description = si.Description,
                    UnitPrice = si.UnitPrice,
                    Quantity = si.Quantity,
                    DiscountAmount = si.DiscountAmount,
                    TaxAmount = si.TaxAmount,
                    LineTotal = si.LineTotal,
                    QuickBooksLineId = si.QuickBooksLineId
                }).ToList()
            };
        }
    }
}