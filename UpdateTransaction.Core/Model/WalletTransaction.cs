using UpdateTransaction.Core.Model.Common;

namespace UpdateTransaction.Core.Model
{
    public class WalletTransaction:BaseEntity
    {
        public Guid ClientId { get; set; }
        public string WalletAddress { get; set; }
        public string CurrencyType { get; set; }
        public DateTime TransactionDate { get; set; }  
        public Client Client { get; set; }
    }
    
}
