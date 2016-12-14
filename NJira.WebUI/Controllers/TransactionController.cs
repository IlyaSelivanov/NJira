using NJira.Domain.Abstract;
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
    public class TransactionController : Controller
    {
        IIssueRepository repository;

        public TransactionController(IIssueRepository issueRepository)
        {
            repository = issueRepository;
        }

        public ActionResult Index(Cart cart)
        {
            Cart c = cart;

            return View(c);
        }

        public RedirectToRouteResult AddToCart(Cart cart, IssueViewModel issuesVM)
        {
            foreach (var issue in issuesVM.Issues.Where(c => c.IsSelected))
                cart.AddItem(issue.Issue);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Transact(IssueViewModel issuesVM)
        {
            foreach (var i in issuesVM.Issues.Where(c => c.IsSelected))
            {
                var issue = repository.Issues.Where(x => x.Key == i.Issue.Key).ElementAt(0);
                await issue.WorkflowTransitionAsync("Back to review");
            }

            return View();
        }
    }
}