using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJira.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Key { get; set; }
        public string Summary { get; set; }
        public string StatusFrom { get; set; }
        public string StatusTo { get; set; }
        public string ResolutionFrom { get; set; }
        public string ResolutionTo { get; set; }
        public string AssigneeFrom { get; set; }
        public string AssigneeTo { get; set; }
        public string Reporter { get; set; }
    }
}
