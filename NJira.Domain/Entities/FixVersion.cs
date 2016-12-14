using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NJira.Domain.Entities
{
    public class FixVersion
    {
        [Required]
        [Display(Name = "Version")]
        public string Name { get; set; }
    }
}
