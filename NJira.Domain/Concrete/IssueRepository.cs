using Atlassian.Jira;
using NJira.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJira.Domain.Concrete
{
    public class IssueRepository : IIssueRepository
    {
        private JiraContext _context;

        public IssueRepository()
        {
            _context = new JiraContext();
        }

        public IQueryable<Issue> Issues
        {
            get { return _context.Issues; }
            set { }
        }
    }
}
