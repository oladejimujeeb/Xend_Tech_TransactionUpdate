using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdateTransaction.Core.DTO;
using UpdateTransaction.Core.Interfaces.RepositoryInterface;
using UpdateTransaction.Core.Interfaces.ServiceInterface;
using UpdateTransaction.Core.Model;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using UpdateTransaction.Core.BusMessage;

namespace UpdateTransaction.Core.Services
{
    public class WalletTransactionService:IWalletTransactionService
    {
        private readonly IWalletTransactionRepository _walletTransactionRepository;
        private readonly ILogger<WalletTransactionService> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ITransactionBusMessage _transactionBusMessage;


        public WalletTransactionService(IWalletTransactionRepository walletTransactionRepository, ILogger<WalletTransactionService> logger, IHttpClientFactory clientFactory, ITransactionBusMessage transactionBusMessage)
        {
            _walletTransactionRepository = walletTransactionRepository;
            _logger = logger;
            _clientFactory = clientFactory;
            _transactionBusMessage = transactionBusMessage;
        }

        public async Task<BaseResponseModel<IEnumerable<AllWalletTransactionVIewModel>>>GetAllWalletTransaction()
        {
            var result = new BaseResponseModel<IEnumerable<AllWalletTransactionVIewModel>>();

            var transaction =await _walletTransactionRepository.Query().Include(x=>x.Client).ToListAsync();
            if(!transaction.Any()) 
            {
                result.Status = false;
                result.Message = "No available infomation";
                return result;
            }
            var data = transaction.Select(t => new AllWalletTransactionVIewModel 
            { 
                ClientId = t.ClientId.ToString(),
                ClientName = t.Client.Name,
                CurrencyType = t.CurrencyType,
                TransactionDate = t.TransactionDate.ToString("d"),
                WalletAddress = t.WalletAddress,
                Id = t.Id.ToString(),
            });
            result.Data = data;
            result.Status = true;
            result.Message = "Success";
            return result;
        }
        public async Task<BaseResponseModel<WalletTransactionVIewModel>>GetWalletTransactionById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new BaseResponseModel<WalletTransactionVIewModel>()
                {
                    Status = false,
                    Message = " Id cannot be null"
                };
            }
            var transaction = await _walletTransactionRepository.Get(id);
            if (transaction is null)
            {
                return new BaseResponseModel<WalletTransactionVIewModel>()
                {
                    Status = false,
                    Message = "Transaction not found"
                };
            }
            return new BaseResponseModel<WalletTransactionVIewModel>()
            {
                Status = true,
                Message = "Transaction not found",
                Data = new WalletTransactionVIewModel() 
                { 
                    ClientId = transaction.ClientId.ToString(), 
                    CurrencyType = transaction.CurrencyType,
                    TransactionDate = transaction.TransactionDate.ToString("d"),
                    WalletAddress = transaction.WalletAddress,
                }
            };

        }
        public async Task<BaseResponseModel<string>>UpdateWalletTransactions(WalletTransactionViewModel model)
        {
            if(model== null)
            {
                return new BaseResponseModel<string>()
                {
                    Data= "Model not valid",
                    Status = false,
                    Message ="Failed"
                };
            }
            if(string.IsNullOrEmpty(model.Wallet_Address) 
                || string.IsNullOrEmpty(model.Wallet_Address)
                || string.IsNullOrEmpty(model.Currency_Type)
                || Guid.Empty == model.Client_Id
            ) 
            {
                return new BaseResponseModel<string>()
                {
                    Data = "Model not valid",
                    Status = false,
                    Message = "Failed"
                };
            }
            var isADuplicateTransaction = await _walletTransactionRepository.Exists(x=>x.ClientId == model.Client_Id && x.WalletAddress== model.Wallet_Address);
            if(!isADuplicateTransaction)
            {
                var cryptoApiCall = await GetTransactionsFromCryptoApi(model);
                if(!cryptoApiCall.Status)
                {
                    return new BaseResponseModel<string>()
                    {
                        Status = false,
                        Message = cryptoApiCall.Message
                    };
                }
                // produce bus message
                await _transactionBusMessage.ProduceDataAsync(cryptoApiCall.Data);
                var transaction = new WalletTransaction 
                { 
                    ClientId = model.Client_Id, 
                    CurrencyType = model.Currency_Type,
                    TransactionDate =DateTime.Now,
                    WalletAddress = model.Wallet_Address,
                };
                var addTransaction = await _walletTransactionRepository.Add(transaction);
                if(addTransaction is null) 
                {
                    _logger.LogError("Failed to save transaction");
                    return new BaseResponseModel<string>()
                    {
                        Message = "Failed to save transaction",
                        Status = false,
                    };
                }
                return new BaseResponseModel<string>()
                {
                    Message = "success",
                    Status = true,
                    Data= "New wallet transaction saved successfull"
                };
            }
            else
            {
                _logger.LogInformation("Duplicate transaction detected. Ignoring the update.");
                return new BaseResponseModel<string>()
                {
                    Message = "Duplicate transaction detected. Ignoring the update.",
                    Status= false,
                };
            }
        }
        public async Task<BaseResponseModel<IEnumerable<AllWalletTransactionVIewModel>>>GetTransactionByClientId(Guid clientId)
        {
            if(clientId == Guid.Empty)
            {
                return new BaseResponseModel<IEnumerable<AllWalletTransactionVIewModel>>()
                {
                    Status = false,
                    Message = "Client Id cannot be null"
                };
            }
            var transaction = await _walletTransactionRepository.Query().Include(x => x.Client).Where(x=>x.ClientId== clientId).ToListAsync();
            if (!transaction.Any())
            {
                return new BaseResponseModel<IEnumerable<AllWalletTransactionVIewModel>>()
                {
                    Status = false,
                    Message = "No available infomation"
                };
            }
            var data = transaction.Select(t => new AllWalletTransactionVIewModel
            {
                ClientId = t.ClientId.ToString(),
                ClientName = t.Client.Name,
                CurrencyType = t.CurrencyType,
                TransactionDate = t.TransactionDate.ToString("d"),
                WalletAddress = t.WalletAddress,
                Id = t.Id.ToString(),
            });
            return new BaseResponseModel<IEnumerable<AllWalletTransactionVIewModel>>()
            {
                Status = true,
                Message = "Success",
                Data = data
            };

        }
        private async Task<BaseResponseModel<string>>GetTransactionsFromCryptoApi(WalletTransactionViewModel model)
        {
            if (model is null)
            {
                return new BaseResponseModel<string> 
                { 
                    Status = false,
                    Message ="model is null"
                };
            }
            var payload = JsonConvert.SerializeObject(model);
            var baseUri = new Uri("www.xendfinance.cryptoapi");
            var client = _clientFactory.CreateClient();
            client.BaseAddress = baseUri;
            var request = new HttpRequestMessage(HttpMethod.Get, $"/transaction/clienttransaction?clientid={model.Client_Id}&walletaddress={model.Wallet_Address}&currencytype{model.Currency_Type}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return new BaseResponseModel<string>
                {
                    Status = false,
                    Message = "Api called failed"
                };
            }
            var transactions = await response.Content.ReadAsStringAsync();
            if(string.IsNullOrEmpty(transactions))
            {
                return new BaseResponseModel<string>
                {
                    Status = false,
                    Message = "Failed to read content"
                };
            }
            return new BaseResponseModel<string>
            {
                Status = false,
                Message = "Success",
                Data = transactions
            };
        }
    }
}
