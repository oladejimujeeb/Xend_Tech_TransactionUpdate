using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdateTransaction.Core.Model.Common;

namespace UpdateTransaction.Core.Model
{
    public class Client:BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<Wallet> ClientWallets { get; set; } = new List<Wallet>();
    }
    
}
