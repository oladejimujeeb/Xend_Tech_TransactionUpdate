using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public WalletTransactionController(IWalletTransactionService walletTransactionService)
        {
            _walletTransactionService = walletTransactionService;
        }
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponseModel<IEnumerable<AllWalletTransactionVIewModel>>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult>WalletTransactions()
        {
            var transaction = await _walletTransactionService.GetAllWalletTransaction();
            if(!transaction.Status)
            {
                return BadRequest(transaction.Message);
            }
            return Ok(transaction);
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
