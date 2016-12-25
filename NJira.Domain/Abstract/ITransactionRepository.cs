using NJira.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJira.Domain.Abstract
{
    public interface ITransactionRepository
    {
        IEnumerable<Transaction> Transactions { get; }
        void SaveTransaction(Transaction tramsaction);
    }
}
