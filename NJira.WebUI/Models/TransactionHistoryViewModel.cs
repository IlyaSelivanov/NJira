using NJira.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NJira.WebUI.Models
{
    public class TransactionHistoryViewModel
    {
        public TransactionHistoryViewModel()
        {
            Transactions = new List<Transaction>();
        }
        public List<Transaction> Transactions { get; set; }
    }
}