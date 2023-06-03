using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdateTransaction.Core.DTO;

namespace UpdateTransaction.Core.Interfaces.ServiceInterface
{
    public interface IClientService
    {
        Task<BaseResponseModel<ClientResponseModel>> CreateClient(CreateClientRequestModel model);
    }
}
