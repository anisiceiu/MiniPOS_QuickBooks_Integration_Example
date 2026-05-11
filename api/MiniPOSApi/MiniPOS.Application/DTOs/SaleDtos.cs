using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiniPOS.Application.DTOs
{
    public class SaleItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? Description { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Quantity { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal? LineTotal { get; set; }
        public string? QuickBooksLineId { get; set; }
    }

    public class CreateSaleItemDto
    {
        [Required]
        public int ProductId { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        public decimal DiscountAmount { get; set; } = 0;

        public decimal TaxAmount { get; set; } = 0;
    }

    public class UpdateSaleItemDto
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        public decimal DiscountAmount { get; set; } = 0;

        public decimal TaxAmount { get; set; } = 0;
    }

    public class SaleDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime SaleDate { get; set; }
        public string? InvoiceNumber { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal ShippingAmount { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
        public string? Notes { get; set; }
        public string PaymentStatus { get; set; }
        public string SyncStatus { get; set; }
        public string? QuickBooksInvoiceId { get; set; }
        public DateTime? LastSyncedAt { get; set; }
        public CustomerDto? Customer { get; set; }
        public List<SaleItemDto> SaleItems { get; set; } = new();
    }

    public class CreateSaleDto
    {
        [Required]
        public int CustomerId { get; set; }

        public DateTime SaleDate { get; set; } = DateTime.UtcNow;

        [MaxLength(50)]
        public string? InvoiceNumber { get; set; }

        public DateTime? DueDate { get; set; }

        public decimal DiscountAmount { get; set; } = 0;

        public decimal TaxAmount { get; set; } = 0;

        public decimal ShippingAmount { get; set; } = 0;

        [MaxLength(10)]
        public string Currency { get; set; } = "BDT";

        [MaxLength(500)]
        public string? Notes { get; set; }

        [MaxLength(20)]
        public string PaymentStatus { get; set; } = "Unpaid";

        [Required]
        public List<CreateSaleItemDto> SaleItems { get; set; } = new();
    }

    public class UpdateSaleDto
    {
        [Required]
        public int CustomerId { get; set; }

        public DateTime SaleDate { get; set; }

        [MaxLength(50)]
        public string? InvoiceNumber { get; set; }

        public DateTime? DueDate { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal ShippingAmount { get; set; }

        [MaxLength(10)]
        public string Currency { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        [MaxLength(20)]
        public string PaymentStatus { get; set; }

        [Required]
        public List<UpdateSaleItemDto> SaleItems { get; set; } = new();
    }
}