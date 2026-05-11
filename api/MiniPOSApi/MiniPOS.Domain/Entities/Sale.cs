using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniPOS.Domain.Entities
{
    [Table("Sales")]
    public class Sale
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public DateTime SaleDate { get; set; }


        [MaxLength(50)]
        public string? InvoiceNumber { get; set; }

        public DateTime? DueDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingAmount { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; } = 0;

        [MaxLength(10)]
        public string Currency { get; set; } = "BDT";

        [MaxLength(500)]
        public string? Notes { get; set; }

        [MaxLength(20)]
        public string PaymentStatus { get; set; } = "Unpaid";

        [MaxLength(20)]
        public string SyncStatus { get; set; } = "Pending";

        [MaxLength(100)]
        public string? QuickBooksInvoiceId { get; set; }

        public DateTime? LastSyncedAt { get; set; }

        // Navigation properties
        [ForeignKey(nameof(CustomerId))]
        public virtual Customer? Customer { get; set; }

        public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
    }
}
