using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdateTransaction.Core.Model;

namespace UpdateTransaction.Core.DTO
{
    public class WalletTransactionViewModel
    {
        public Guid Client_Id { get; set; }
        public string Wallet_Address { get; set; }
        public string Currency_Type { get; set; }
    }
}
