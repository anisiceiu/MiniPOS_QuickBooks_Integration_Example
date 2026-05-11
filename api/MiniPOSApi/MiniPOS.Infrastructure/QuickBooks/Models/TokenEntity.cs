using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniPOS.Infrastructure.QuickBooks.Models
{
    public class TokenEntity
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string RealmId { get; set; }
        public DateTime Expiry { get; set; }

        public bool IsExpired => DateTime.UtcNow >= Expiry;
    }
}
