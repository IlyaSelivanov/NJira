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
    [Authorize]
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
            ViewBag.DataPoints = JsonConvert.SerializeObject(GetRandomDataForCategoryAxis(5), _jsonSetting);

            return View();
        }

        private static List<DataPoint> GetRandomDataForCategoryAxis(int count)
        {
            List<DataPoint> _dataPoints = new List<DataPoint>();
            Random random = new Random(DateTime.Now.Millisecond);

            double y = 50;
            DateTime dateTime = new DateTime(2006, 01, 1, 0, 0, 0);
            string label = "";

            _dataPoints = new List<DataPoint>();


            for (int i = 0; i < count; i++)
            {
                y = y + (random.Next(0, 20) - 10);
                label = dateTime.ToString("dd MMM");

                _dataPoints.Add(new DataPoint(y, label));
                dateTime = dateTime.AddDays(1);
            }

            return _dataPoints;
        }
    }
}