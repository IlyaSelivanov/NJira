using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NJira.Domain.Entities;
using System.Web.Mvc;

namespace NJira.WebUI.Models
{
    public class TransactionSettingsViewModel
    {
        public string Version { get; set; }
        public string Type { get; set; }
        public string Resolution { get; set; }
        public string CodeLocation { get; set; }
        public string Comment { get; set; }

        public IEnumerable<SelectListItem> Versions { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }
        public IEnumerable<SelectListItem> Resolutions { get; set; }
        public IEnumerable<SelectListItem> CodeLocations { get; set; }
    }
}