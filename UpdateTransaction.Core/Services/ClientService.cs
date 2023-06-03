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
using UpdateTransaction.Core.Repositories;

namespace UpdateTransaction.Core.Services
{
    public class ClientService:IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly ILogger<ClientService> _logger;

        public ClientService( IClientRepository clientRepository, IWalletRepository walletRepository, ILogger<ClientService> logger)
        {
            _clientRepository = clientRepository;
            _walletRepository = walletRepository;
            _logger = logger;
        }

        public async Task<BaseResponseModel<ClientResponseModel>> CreateClient(CreateClientRequestModel model)
        {
            var result = new BaseResponseModel<ClientResponseModel>();
            if(model == null ) 
            {
                result.Status = false;
                result.Message = "Create Model cannot be null";
                return result;
            }

            var client = new Client
            {
                Address = model!.ClientAddress,
                Country = model!.Country,
                Name = model.ClientName,
                PhoneNumber = model!.PhoneNumber,
            };
            var defaultwallet = new Wallet 
            { 
                ClientId = client.Id,
                WalletAddress = $"{Guid.NewGuid().ToString().Replace("-","").ToUpper()}",
                WalletBalance = 0.0f,
                CurrencyType ="BTC"
            };
            client.ClientWallets.Add(defaultwallet);
            var addClient = await _clientRepository.Add(client);
            if(addClient== null) 
            {
                result.Status = false;
                result.Message = "Failed";
                return result;
            }
            result.Status = true;
            result.Message = "Success";
            result.Data = new ClientResponseModel 
            { 
                ClientId = addClient.Id.ToString(),
                ClientName = addClient.Name,
            };

            return result;
        }
     }
}
