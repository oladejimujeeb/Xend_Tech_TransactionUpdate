using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UpdateTransaction.Core.DTO;
using UpdateTransaction.Core.Interfaces.ServiceInterface;
using UpdateTransaction.Core.Services;

namespace TransactionUpdateAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IClientService _clientService;
        public UserController(IClientService clientService)
        {
            _clientService = clientService;
        }
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponseModel<ClientResponseModel>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult>CreateUser(CreateClientRequestModel model)
        {
            if(!ModelState.IsValid) 
            {
                return BadRequest($"Invalid Model{nameof(model)}"); 
            }
            var transaction = await _clientService.CreateClient(model);
            if (!transaction.Status)
            {
                return BadRequest(transaction.Message);
            }
            return Ok(transaction);
        }
    }
}
