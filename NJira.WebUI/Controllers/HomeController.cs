using NJira.Domain.Concrete;
using NJira.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NJira.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private JiraContext jiraContext = new JiraContext();

        // GET: Home
        public async Task<ActionResult> Index()
        {
            IEnumerable<Atlassian.Jira.ProjectVersion> versions;
            IEnumerable<Atlassian.Jira.IssueStatus> statuses;

            try
            {
                versions = await jiraContext.Jira.Versions.GetVersionsAsync("PVINE");
                statuses = await jiraContext.Jira.Statuses.GetStatusesAsync();
            }
            catch(Exception ex)
            {
                var exc = ex.Message;
                return RedirectToAction("Oops", "Error");
            }

            var searchModel = new SearchModel();

            var verList = new List<SelectListItem>();
            foreach(var version in versions.Where(v => v.IsReleased == false).OrderByDescending(v => v.Id))
            {
                verList.Add(new SelectListItem
                {
                    Value = version.Name,
                    Text = version.Name
                });
            }
            searchModel.Versions = verList;

            var statList = new List<SelectListItem>();

            statList.Add(new SelectListItem
            {
                Value = "All",
                Text = "All"
            });

            foreach (var status in statuses.OrderBy(s => s.Name))
            {
                statList.Add(new SelectListItem
                {
                    Value = status.Name,
                    Text = status.Name
                });
            }
            searchModel.Statuses = statList;

            if (searchModel.Statuses == null || searchModel.Versions == null)
                return RedirectToAction("Oops", "Error");

            return View(searchModel);
        }

        [HttpPost]
        public ActionResult Index(SearchModel searchModel)
        {
            if (ModelState.IsValid)
                return RedirectToAction("Index", "Issue", searchModel);

            return View();
        }
    }
}