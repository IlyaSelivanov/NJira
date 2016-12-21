using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJira.Domain.Entities
{
    public class Issue
    {
        public string Key { get; set; }
        public string Summary { get; set; }
        public string Status { get; set; }
    }
}
