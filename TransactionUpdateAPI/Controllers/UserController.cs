using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using UpdateTransaction.Core.DTO;
using UpdateTransaction.Core.Interfaces.ServiceInterface;
using UpdateTransaction.Core.NotificationHub;
using UpdateTransaction.Core.Services;

namespace TransactionUpdateAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IHubContext<MessageHub, IMessageHubClient> _message;
        public UserController(IClientService clientService, IHubContext<MessageHub, IMessageHubClient> message)
        {
            _clientService = clientService;
            _message = message;
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
            List<string> notify = new List<string>() 
            { 
                "New User created",
                model.ClientName, model.ClientAddress, model.PhoneNumber, model.Country
            };
             await _message.Clients.All.NewUserAdded(notify);
            return Ok(transaction);
        }
        private static readonly string[] Summaries = new[]
       {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

       
        [HttpGet]
        public IActionResult WeatherForecasts()
        {
            var rng = new Random();
            var weather = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
            _message.Clients.All.WeatherReport(weather);
            return Ok(weather);
        }
    }
}
