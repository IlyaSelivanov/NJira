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
        IJiraRepository repository;

        public TransactionController(IJiraRepository jiraRepository)
        {
            repository = jiraRepository;
        }

        public async Task<ActionResult> Index(Cart cart)
        {
            Cart c = cart;
            ViewBag.Count = c.Issues.Count;

            List<string> versions = new List<string>();
            List<string> trType = new List<string>();
            List<Tuple<string, string>> resolutions = new List<Tuple<string, string>>();

            try
            {
                versions = await repository.GetVersionsAsync("PVINE");
                trType = await repository.GetAvailableStatusesAsync(cart.Issues.First().Key);
                resolutions = await repository.GetResolutionsAsync();
            }
            catch (Exception ex)
            {
                var exc = ex.Message;
                return RedirectToAction("Oops", "Error");
            }

            TransactionSettingsViewModel settings = new TransactionSettingsViewModel();

            var verList = new List<SelectListItem>();
            foreach (var version in versions)
            {
                verList.Add(new SelectListItem
                {
                    Value = version,
                    Text = version
                });
            }
            settings.Versions = verList;

            var typeList = new List<SelectListItem>();
            foreach (var type in trType)
            {
                typeList.Add(new SelectListItem
                {
                    Value = type,
                    Text = type
                });
            }
            settings.Types = typeList;

            var resList = new List<SelectListItem>();
            foreach (var resolution in resolutions)
            {
                resList.Add(new SelectListItem
                {
                    Value = resolution.Item1,
                    Text = resolution.Item2
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

                issue.Resolution = new Atlassian.Jira.IssueResolution(settings.Resolution);
                issue.Assignee = issue.Reporter;
                await issue.AddCommentAsync(settings.Comment);
                await issue.WorkflowTransitionAsync(settings.Type);
                await issue.SaveChangesAsync();
            }

            cart.Issues.Clear();

            return View();
        }
    }
}