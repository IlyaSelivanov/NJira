using NJira.Domain.Abstract;
using NJira.Domain.Entities;
using NJira.WebUI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NJira.WebUI.Controllers
{
    [Authorize]
    public class IssueController : Controller
    {
        IJiraRepository repository;

        public IssueController(IJiraRepository issueRepository)
        {
            repository = issueRepository;
        }

        // GET: Issue
        public ActionResult Index(SearchViewModel searchModel, Cart cart)
        {
            cart.ClearCart();

            IssueViewModel issuesVM = new IssueViewModel();

            if (searchModel.Status == null && searchModel.Version == null)
                return RedirectToAction("Index", "Home");

            var issues = from i in repository.Issues
                         where i.FixVersions == searchModel.Version
                         select i;

            if (searchModel.Status != null && !searchModel.Status.Equals("All"))
                issues = issues.Where(i => i.Status == searchModel.Status);

            ViewBag.Count = issues.Count();

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

            return View(issuesVM);
        }
    }
}