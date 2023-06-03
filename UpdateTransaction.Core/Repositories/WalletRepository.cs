using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdateTransaction.Core.Context;
using UpdateTransaction.Core.Interfaces.RepositoryInterface;
using UpdateTransaction.Core.Model;

namespace UpdateTransaction.Core.Repositories
{
    public class WalletRepository:BaseRepository<Wallet>, IWalletRepository
    {
        public WalletRepository(AppDbContext context)
        {
            _context = context;
        }
    }
}
