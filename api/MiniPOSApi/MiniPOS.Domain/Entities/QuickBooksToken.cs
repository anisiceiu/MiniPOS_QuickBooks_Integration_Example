using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniPOS.Domain.Entities
{
    [Table("QuickBooksTokens")]
    public class QuickBooksToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? AccessToken { get; set; }

        public string? RefreshToken { get; set; }

        [MaxLength(100)]
        public string? RealmId { get; set; }

        public DateTime? AccessTokenExpiresAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
