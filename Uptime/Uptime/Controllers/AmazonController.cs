using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Uptime.Models;

namespace Uptime.Controllers
{
    public class AmazonController : Controller
    {
        public List<List<List<string>>> keepList;
        // GET: Amazon
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public ActionResult AmazonSearch(APIViewModels api)
        {
            api.finalList = api.keyword(api.sona);
            Session["list"] = api.finalList;
            return View("AmazonSearch", api);
        }

        
        public ActionResult AmazonSearch1(APIViewModels api)
        {
            api.finalList = Session["list"] as List<List<List<string>>>;
            return View("AmazonSearch", api);
        }

        public ActionResult AmazonSearch2(APIViewModels api)
        {
            api.finalList = Session["list"] as List<List<List<string>>>;
            return View("AmazonSearch2", api);
        }

        public ActionResult AmazonSearch3(APIViewModels api)
        {
            api.finalList = Session["list"] as List<List<List<string>>>;
            return View("AmazonSearch3", api);
        }

        public ActionResult AmazonSearch4(APIViewModels api)
        {
            api.finalList = Session["list"] as List<List<List<string>>>;
            return View("AmazonSearch4", api);
        }
    }

        
}