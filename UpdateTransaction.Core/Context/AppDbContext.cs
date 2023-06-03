using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdateTransaction.Core.Model;

namespace UpdateTransaction.Core.Context
{
    public class AppDbContext:DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
                
        }

        public DbSet<Client> clients { get; set; }
        public DbSet<Wallet> wallets { get; set; }
        public DbSet<WalletTransaction> walletTransactions { get; set; }
    }
}
