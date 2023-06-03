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
    public class ClientRepository:BaseRepository<Client>, IClientRepository
    {
        public ClientRepository(AppDbContext context) 
        { 
            _context = context;
        }
    }
}
