using NJira.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJira.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public EFDbContext() : base("NJiraDb") {  }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
