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
    public class HomeController : Controller
    {
        IJiraRepository repository;

        public HomeController(IJiraRepository issueRepository)
        {
            repository = issueRepository;
        }

        // GET: Home
        public async Task<ActionResult> Index()
        {
            //List<string> versions;
            //List<string> statuses;

            //try
            //{
            //    versions = await repository.GetVersionsAsync("PVINE");
            //    statuses = await repository.GetStatusesAsync();
            //}
            //catch(Exception ex)
            //{
            //    var exc = ex.Message;
            //    return RedirectToAction("Oops", "Error");
            //}

            //var searchModel = new SearchViewModel();

            //var verList = new List<SelectListItem>();
            //foreach(var version in versions)
            //{
            //    verList.Add(new SelectListItem
            //    {
            //        Value = version,
            //        Text = version
            //    });
            //}
            //searchModel.Versions = verList;

            //var statList = new List<SelectListItem>();

            //foreach (var status in statuses)
            //{
            //    statList.Add(new SelectListItem
            //    {
            //        Value = status,
            //        Text = status
            //    });
            //}
            //searchModel.Statuses = statList;

            SearchViewModel searchModel = await InitSearchModel();

            if (searchModel.Statuses == null || searchModel.Versions == null)
                return RedirectToAction("Oops", "Error");

            return View(searchModel);
        }

        [HttpPost]
        public ActionResult Index(SearchViewModel searchModel)
        {
            if (ModelState.IsValid)
                return RedirectToAction("Index", "Issue", new { searchModel.Version, searchModel.Status });

            return View();
        }

        private async Task<SearchViewModel> InitSearchModel()
        {
            SearchViewModel model = new SearchViewModel();

            List<string> versions = new List<string>();
            List<string> statuses = new List<string>();

            try
            {
                versions = await repository.GetVersionsAsync("PVINE");
                statuses = await repository.GetStatusesAsync();
            }
            catch (Exception ex)
            {
                var exc = ex.Message;
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