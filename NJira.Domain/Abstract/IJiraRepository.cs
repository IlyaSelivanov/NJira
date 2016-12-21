using Atlassian.Jira;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJira.Domain.Abstract
{
    public interface IJiraRepository
    {
        IQueryable<Issue> Issues { get; }
        Task<List<string>> GetVersionsAsync(string project);
        Task<List<string>> GetStatusesAsync();
        Task<List<Tuple<string, string>>> GetResolutionsAsync();
        Task<List<string>> GetAvailableStatusesAsync(string key);
    }
}
