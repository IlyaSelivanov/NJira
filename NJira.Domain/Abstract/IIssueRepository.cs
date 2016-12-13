using Atlassian.Jira;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJira.Domain.Abstract
{
    public interface IIssueRepository
    {
        IQueryable<Issue> Issues { get; set; }
    }
}
