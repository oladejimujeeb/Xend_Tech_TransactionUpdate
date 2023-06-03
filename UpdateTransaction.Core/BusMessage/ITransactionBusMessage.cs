using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateTransaction.Core.BusMessage
{
    public interface ITransactionBusMessage
    {
        Task<bool> ProduceDataAsync(string transactions);
        Task<string> ConsumeDataAsync( string message);
    }
}
