using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdateTransaction.Core.DTO;

namespace UpdateTransaction.Core.Interfaces.ServiceInterface
{
    public interface IWalletTransactionService
    {
        Task<BaseResponseModel<IEnumerable<AllWalletTransactionVIewModel>>> GetAllWalletTransaction();
        Task<BaseResponseModel<string>> UpdateWalletTransactions(WalletTransactionViewModel model);
        Task<BaseResponseModel<IEnumerable<AllWalletTransactionVIewModel>>> GetTransactionByClientId(Guid clientId);
        Task<BaseResponseModel<WalletTransactionVIewModel>> GetWalletTransactionById(Guid id);
    }
}
