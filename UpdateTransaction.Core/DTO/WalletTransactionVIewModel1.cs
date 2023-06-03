namespace UpdateTransaction.Core.DTO
{
    public class WalletTransactionVIewModel
    {
        public string ClientId { get; set; }
        public string WalletAddress { get; set; }
        public string CurrencyType { get; set; }
        public string TransactionDate { get; set; }
    }
}
