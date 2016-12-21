using Atlassian.Jira;
using NJira.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJira.Domain.Concrete
{
    public class JiraRepository : IJiraRepository
    {
        private JiraContext _context = new JiraContext();

        public IQueryable<Issue> Issues
        {
            get { return _context.Jira.Issues.Queryable; }
        }

        public async Task<List<string>> GetVersionsAsync(string project)
        {
            List<string> verNames = new List<string>();

            var versions = await _context.Jira.Versions.GetVersionsAsync(project);

            foreach (var v in versions.Where(x => x.IsReleased == false).OrderByDescending(x => x.Id))
                verNames.Add(v.Name);

            return verNames;
        }

        public async Task<List<string>> GetStatusesAsync()
        {
            List<string> statNames = new List<string>();

            var statuses = await _context.Jira.Statuses.GetStatusesAsync();

            foreach (var status in statuses.OrderBy(s => s.Name))
                statNames.Add(status.Name);

            return statNames;
        }

        public async Task<List<Tuple<string, string>>> GetResolutionsAsync()
        {
            List<Tuple<string, string>> resNames = new List<Tuple<string, string>>();

            var resolutions = await _context.Jira.Resolutions.GetResolutionsAsync();

            foreach (var resolution in resolutions.OrderBy(s => s.Name))
            {
                Tuple<string, string> res = new Tuple<string, string>(resolution.Id, resolution.Name);
                resNames.Add(res);
            }

            return resNames;
        }

        public async Task<List<string>> GetAvailableStatusesAsync(string key)
        {
            List<string> statList = new List<string>();
            var available = await Issues.Where(i => i.Key == key).First().GetAvailableActionsAsync();

            foreach (var status in available)
                statList.Add(status.Name);

            return statList;
        }
    }
}
