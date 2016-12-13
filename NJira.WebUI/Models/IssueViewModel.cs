using NJira.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NJira.WebUI.Models
{
    public class IssueLine
    {
        public Issue Issue { get; set; }
        public bool IsSelected { get; set; }
    }

    public class IssueViewModel
    {
        public IssueViewModel()
        {
            Issues = new List<IssueLine>();
        }

        public List<IssueLine> Issues { get; set; }
    }
}