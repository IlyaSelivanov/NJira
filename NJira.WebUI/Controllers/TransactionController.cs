using NJira.Domain.Abstract;
using NJira.Domain.Concrete;
using NJira.Domain.Entities;
using NJira.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NJira.WebUI.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        IIssueRepository repository;
        JiraContext jiraContext = new JiraContext();

        public TransactionController(IIssueRepository issueRepository)
        {
            repository = issueRepository;
        }

        public async Task<ActionResult> Index(Cart cart)
        {
            Cart c = cart;
            ViewBag.Count = c.Issues.Count;

            IEnumerable<Atlassian.Jira.ProjectVersion> versions;
            IEnumerable<Atlassian.Jira.JiraNamedEntity> trType;
            IEnumerable<Atlassian.Jira.IssueResolution> resolutions;

            try
            {
                versions = await jiraContext.Jira.Versions.GetVersionsAsync("PVINE");
                trType = await repository.Issues.Where(i => i.Key == cart.Issues.First().Key).First().GetAvailableActionsAsync();
                resolutions = await jiraContext.Jira.Resolutions.GetResolutionsAsync();
            }
            catch (Exception ex)
            {
                var exc = ex.Message;
                return RedirectToAction("Oops", "Error");
            }

            TransactionSettingsViewModel settings = new TransactionSettingsViewModel();

            var verList = new List<SelectListItem>();
            foreach (var version in versions.Where(v => v.IsReleased == false).OrderByDescending(v => v.Id))
            {
                verList.Add(new SelectListItem
                {
                    Value = version.Name,
                    Text = version.Name
                });
            }
            settings.Versions = verList;

            var typeList = new List<SelectListItem>();
            foreach (var type in trType.OrderBy(s => s.Name))
            {
                typeList.Add(new SelectListItem
                {
                    Value = type.Name,
                    Text = type.Name
                });
            }
            settings.Types = typeList;

            var resList = new List<SelectListItem>();
            foreach (var resolution in resolutions.OrderBy(s => s.Name))
            {
                resList.Add(new SelectListItem
                {
                    Value = resolution.Id,
                    Text = resolution.Name
                });
            }
            settings.Resolutions = resList;

            var locations = new List<SelectListItem>();
            locations.Add(new SelectListItem
            {
                Value = "QA",
                Text = "QA"
            });
            locations.Add(new SelectListItem
            {
                Value = "Dev54",
                Text = "Dev54"
            });
            locations.Add(new SelectListItem
            {
                Value = "Stage",
                Text = "Stage"
            });
            locations.Add(new SelectListItem
            {
                Value = "Live",
                Text = "Live"
            });
            settings.CodeLocations = locations;


            return View(settings);
        }

        public RedirectToRouteResult AddToCart(Cart cart, IssueViewModel issuesVM)
        {
            foreach (var issue in issuesVM.Issues.Where(c => c.IsSelected))
                cart.AddItem(issue.Issue);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Transact(Cart cart, TransactionSettingsViewModel settings)
        {
            foreach (var i in cart.Issues)
            {
                var issue = repository.Issues.Where(x => x.Key == i.Key).First();

                if (!settings.CodeLocation.Equals(string.Empty))
                {
                    if (!issue.CustomFields.Contains(issue.CustomFields["Code location"]))
                        issue.CustomFields.Add("Code location", settings.CodeLocation);
                    else
                        issue.CustomFields["Code location"].Values = new string[] { settings.CodeLocation };
                }

                issue.Resolution = new Atlassian.Jira.IssueResolution(settings.Resolution.Name);
                issue.Assignee = issue.Reporter;
                await issue.AddCommentAsync(settings.Comment);
                await issue.WorkflowTransitionAsync(settings.Type.Name);
                await issue.SaveChangesAsync();
            }

            cart.Issues.Clear();

            return View();
        }
    }
}