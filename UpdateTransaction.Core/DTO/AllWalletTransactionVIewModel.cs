using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdateTransaction.Core.Model;

namespace UpdateTransaction.Core.DTO
{
    public class AllWalletTransactionVIewModel
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string WalletAddress { get; set; }
        public string CurrencyType { get; set; }
        public string TransactionDate { get; set; }
    }
}
