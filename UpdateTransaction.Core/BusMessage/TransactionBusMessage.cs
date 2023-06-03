using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateTransaction.Core.BusMessage
{
    public class TransactionBusMessage : ITransactionBusMessage
    {
        public Task<string> ConsumeDataAsync(string message)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ProduceDataAsync(string transactions)
        {
            throw new NotImplementedException();
        }
    }
}
