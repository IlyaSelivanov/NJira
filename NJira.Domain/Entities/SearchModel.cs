using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NJira.Domain.Entities
{
    public class SearchModel
    {
        [Required]
        [Display(Name = "Version")]
        public string Version { get; set; }

        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; }

        public IEnumerable<SelectListItem> Versions { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }
    }
}
