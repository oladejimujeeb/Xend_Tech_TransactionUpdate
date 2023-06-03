using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdateTransaction.Core.BusMessage;
using UpdateTransaction.Core.DTO;
using UpdateTransaction.Core.Interfaces.ServiceInterface;

namespace UpdateTransaction.Core.EventHandler
{
    public class UpdateTransactionEvent
    {
        private readonly ITransactionBusMessage _messageBus;
        private readonly ILogger _logger;
        private readonly IWalletTransactionService _walletTransactionService;
        public UpdateTransactionEvent(ITransactionBusMessage messageBus, ILogger logger, IWalletTransactionService walletTransactionService)
        {
            _messageBus = messageBus;
            _logger = logger;
            _walletTransactionService = walletTransactionService;
        }

        public void UpdateTransactionCommandHandler( string message)
        {
            var response = _messageBus.ConsumeDataAsync(message).Result;
            var model = JsonConvert.DeserializeObject<WalletTransactionViewModel>(message);
            if (model is null)
            {
                _logger.LogError("Invalid model");
                return;
            }
            var updateWalletTransaction = _walletTransactionService.UpdateWalletTransactions(model).Result;
            if (!updateWalletTransaction.Status)
            {
                _logger.LogError(updateWalletTransaction.Message);
            }
        }
    }
}
