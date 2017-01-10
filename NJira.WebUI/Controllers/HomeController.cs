using Newtonsoft.Json;
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
    //[Authorize]
    public class HomeController : Controller
    {
        IJiraRepository repository;
        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };

        public HomeController(IJiraRepository issueRepository)
        {
            repository = issueRepository;
        }

        // GET: Home
        public ActionResult Index()
        {
            var issues = from i in repository.Issues
                         where i.FixVersions == "1.32.5"
                         orderby i.Status
                         select i;

            string status = "";
            double total = (double)issues.Count();
            double count = 0;
            List<Tuple<string, double>> result = new List<Tuple<string, double>>();

            foreach (var issue in issues)
            {
                if (!issue.Status.Name.Equals(status))
                {
                    result.Add(new Tuple<string, double>( status, count));
                    status = issue.Status.Name;
                    count = 1;
                }
                else
                    count++;
            }
            result.Add(new Tuple<string, double>(status, count));
            result.RemoveAt(0);

            ViewBag.DataPoints = JsonConvert.SerializeObject(GetPieData(result, total), _jsonSetting);

            return View();
        }

        private static List<DataPoint> GetPieData(List<Tuple<string, double>> result, double count)
        {
            List<DataPoint> _dataPoints = new List<DataPoint>();

            foreach(var res in result)
            {
                _dataPoints.Add(new DataPoint(Math.Round((res.Item2 / count) * 100, 2),
                    res.Item1, 
                    string.Format("({0})", res.Item2.ToString())));
            }

            return _dataPoints;
        }
    }
}