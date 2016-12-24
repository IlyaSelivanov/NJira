using NJira.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJira.Domain.Concrete
{
    public class EFTransactionRepository
    {
        private EFDbContext context = new EFDbContext();
        public IEnumerable<Transaction> Transactions
        {
            get { return context.Transactions; }
        }

        public void SaveTransaction (Transaction tramsaction)
        {
            if(tramsaction.Id == 0)
                context.Transactions.Add(tramsaction);

            context.SaveChanges();
        }
    }
}
