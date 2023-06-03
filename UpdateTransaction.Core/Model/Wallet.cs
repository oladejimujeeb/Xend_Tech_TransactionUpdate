using UpdateTransaction.Core.Model.Common;

namespace UpdateTransaction.Core.Model
{
    public class Wallet : BaseEntity
    {
        public Guid ClientId { get; set; }
        public Client Client { get; set; }
        public string WalletAddress { get; set; }
        public string CurrencyType { get; set; }
        public double WalletBalance { get; set; }

    }

}
