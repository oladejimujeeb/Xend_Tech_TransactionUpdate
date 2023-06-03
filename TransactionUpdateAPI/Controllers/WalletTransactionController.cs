using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.ComponentModel.DataAnnotations;
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
        private const string Key = "transactionCache";

        public WalletTransactionController(IWalletTransactionService walletTransactionService, IMemoryCache memoryCache)
        {
            _walletTransactionService = walletTransactionService;
            _memoryCache = memoryCache;
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
