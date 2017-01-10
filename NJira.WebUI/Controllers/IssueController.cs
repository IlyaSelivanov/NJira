using NJira.Domain.Abstract;
using NJira.Domain.Entities;
using NJira.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NJira.WebUI.Controllers
{
    [Authorize]
    public class IssueController : Controller
    {
        IJiraRepository repository;
        string error;

        public IssueController(IJiraRepository issueRepository)
        {
            repository = issueRepository;
            error = string.Empty;
        }

        // GET: Issue
        public async Task<ActionResult> Index(SearchViewModel searchModel, Cart cart)
        {
            cart.ClearCart();

            IssueViewModel issuesVM = new IssueViewModel();

            if (searchModel.Status != null && searchModel.Version != null)
            {
                var issues = from i in repository.Issues
                             where i.FixVersions == searchModel.Version
                             select i;

                if (searchModel.Status != null && !searchModel.Status.Equals("All"))
                    issues = issues.Where(i => i.Status == searchModel.Status);

                if (searchModel.Assignee != null)
                    issues = issues.Where(i => i.Assignee == searchModel.Assignee);

                foreach (var issue in issues.OrderBy(i => i.Assignee))
                {
                    issuesVM.Issues.Add(new IssueLine
                    {
                        Issue = new Issue
                        {
                            Key = issue.Key.Value,
                            Summary = issue.Summary,
                            Status = issue.Status.Name,
                            Resolution = (issue.Resolution == null) ? "Unresolved" : issue.Resolution.Name,
                            Assignee = issue.Assignee,
                            Reporter = issue.Reporter
                        },
                        IsSelected = false
                    });
                }
                ViewBag.Count = issues.Count();
            }
            
            var model = await InitSearchModel();

            if (model != null)
                ViewBag.SearchModel = model;
            else
            {
                TempData["Error"] = error;
                return RedirectToAction("Oops", "Error");
            }

            return View(issuesVM);
        }

        public ActionResult Partial(SearchViewModel searchModel)
        {
            return PartialView(searchModel);
        }

        private async Task<SearchViewModel> InitSearchModel()
        {
            SearchViewModel model = new SearchViewModel();

            List<string> versions = new List<string>();
            List<string> statuses = new List<string>();
            List<string> users = new List<string>();

            try
            {
                versions = await repository.GetVersionsAsync("PVINE");
                statuses = await repository.GetStatusesAsync();
            }
            catch (Exception ex)
            {
                error = String.Format("Exception: {0}", ex.Message);
                return null;
            }

            var verList = new List<SelectListItem>();
            foreach (var version in versions)
            {
                verList.Add(new SelectListItem
                {
                    Value = version,
                    Text = version
                });
            }
            model.Versions = verList;

            var statList = new List<SelectListItem>();

            foreach (var status in statuses)
            {
                statList.Add(new SelectListItem
                {
                    Value = status,
                    Text = status
                });
            }
            model.Statuses = statList;

            return model;
        }
    }
}