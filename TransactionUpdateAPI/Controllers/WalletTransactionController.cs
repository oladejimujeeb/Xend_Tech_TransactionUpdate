using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UpdateTransaction.Core.DTO;
using UpdateTransaction.Core.Interfaces.ServiceInterface;

namespace TransactionUpdateAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WalletTransactionController : ControllerBase
    {
        private readonly IWalletTransactionService _walletTransactionService;
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;
        private const string Key = "transactionCache";

        public WalletTransactionController(IWalletTransactionService walletTransactionService, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            _walletTransactionService = walletTransactionService;
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
        }
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponseModel<IEnumerable<AllWalletTransactionVIewModel>>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult>WalletTransactions()
        {
            BaseResponseModel<IEnumerable<AllWalletTransactionVIewModel>> transactions;
            if (!_memoryCache.TryGetValue(Key, out transactions))
            {
                transactions = await _walletTransactionService.GetAllWalletTransaction(); //Get the data from database

                var option = new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(90),
                    Size = 1024,
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3)
                };

                _memoryCache.Set(Key, transactions, option);
                if (!transactions.Status)
                {
                    return BadRequest(transactions.Message);
                }
                return Ok(transactions);
            }
            Thread.Sleep(2000);
            if (!transactions.Status)
            {
                return BadRequest(transactions.Message);
            }
            return Ok(transactions);
        }
        [HttpGet("reddis")]
        [ProducesResponseType(typeof(BaseResponseModel<IEnumerable<AllWalletTransactionVIewModel>>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> WalletTransactionsReddis()
        {
            BaseResponseModel<IEnumerable<AllWalletTransactionVIewModel>>? transactions;
            string? serializeWalletTransaction = null;
            var reddisWalletTransaction = await _distributedCache.GetAsync(Key);
            if (reddisWalletTransaction is not null)
            {
                serializeWalletTransaction = Encoding.UTF8.GetString(reddisWalletTransaction);
                BaseResponseModel<IEnumerable<AllWalletTransactionVIewModel>>? baseResponseModel = JsonConvert.DeserializeObject<BaseResponseModel<IEnumerable<AllWalletTransactionVIewModel>>>(serializeWalletTransaction);
                transactions = baseResponseModel;
            }
            else
            {
                transactions = await _walletTransactionService.GetAllWalletTransaction(); //Get the data from database
                serializeWalletTransaction = JsonConvert.SerializeObject(transactions);

                reddisWalletTransaction = Encoding.UTF8.GetBytes(serializeWalletTransaction);
                var options = new DistributedCacheEntryOptions()
                {
                    SlidingExpiration = TimeSpan.FromMinutes(3),
                    AbsoluteExpiration = DateTime.Now.AddMinutes(10)
                };
                await _distributedCache.SetAsync(Key,reddisWalletTransaction, options);                 
            }
           
            return Ok(transactions);
        }
        [HttpGet("{transactionId}")]
        [ProducesResponseType(typeof(BaseResponseModel<WalletTransactionVIewModel>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> WalletTransaction([Required(ErrorMessage ="TransactionId is required")] Guid transactionId)
        {
            var transaction = await _walletTransactionService.GetWalletTransactionById(transactionId);
            if (!transaction.Status)
            {
                return BadRequest(transaction.Message);
            }
            return Ok(transaction);
        }
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(BaseResponseModel<IEnumerable<AllWalletTransactionVIewModel>>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> WalletTransactionByUserId([Required(ErrorMessage = "clientId is required")] Guid userId)
        {
            var transaction = await _walletTransactionService.GetTransactionByClientId(userId);
            if (!transaction.Status)
            {
                return BadRequest(transaction.Message);
            }
            return Ok(transaction);
        }
    }
}
