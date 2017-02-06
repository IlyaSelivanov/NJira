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
                         where i.FixVersions == "1.32.7"
                         orderby i.Status
                         select i;

            string text = "";
            double total = (double)issues.Count();
            double count = 0;
            List<Tuple<string, double>> result = new List<Tuple<string, double>>();

            foreach (var issue in issues)
            {
                if (!issue.Status.Name.Equals(text))
                {
                    result.Add(new Tuple<string, double>( text, count));
                    text = issue.Status.Name;
                    count = 1;
                }
                else
                    count++;
            }
            result.Add(new Tuple<string, double>(text, count));
            result.RemoveAt(0);

            ViewBag.DataPoints = JsonConvert.SerializeObject(GetPieData(result, total), _jsonSetting);

            text = "";
            count = 0;
            result.Clear();
            foreach (var issue in issues.Where(i => i.Status == "Open").OrderBy(i => i.Assignee))
            {
                if (!issue.Assignee.Equals(text))
                {
                    result.Add(new Tuple<string, double>(text, count));
                    text = issue.Assignee;
                    count = 1;
                }
                else
                    count++;
            }
            result.Add(new Tuple<string, double>(text, count));
            result.RemoveAt(0);

            ViewBag.DataPoints1 = JsonConvert.SerializeObject(GetColumnData(result, total), _jsonSetting);

            text = "";
            count = 0;
            result.Clear();
            foreach (var issue in issues.Where(i => i.Status == "Waiting for Review").OrderBy(i => i.Assignee))
            {
                if (!issue.Assignee.Equals(text))
                {
                    result.Add(new Tuple<string, double>(text, count));
                    text = issue.Assignee;
                    count = 1;
                }
                else
                    count++;
            }
            result.Add(new Tuple<string, double>(text, count));
            result.RemoveAt(0);

            ViewBag.DataPoints2 = JsonConvert.SerializeObject(GetColumnData(result, total), _jsonSetting);

            ViewBag.Total = total;

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

        private static List<DataPoint> GetColumnData(List<Tuple<string, double>> result, double count)
        {
            List<DataPoint> _dataPoints = new List<DataPoint>();

            foreach (var res in result)
            {
                _dataPoints.Add(new DataPoint(res.Item2,
                    res.Item1,
                    res.Item1));
            }

            return _dataPoints;
        }
    }
}