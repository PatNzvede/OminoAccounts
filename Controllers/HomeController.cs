using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BarcodeApp.Models;
using Newtonsoft.Json.Linq;
using TestDetails;
namespace BarcodeApp.Controllers
{
    public class HomeController : Controller
    {
        static HttpClient client = new HttpClient();
        public ActionResult Index()
        {
         return View();
        }
        [HttpPost]
        public ActionResult Index(Models.stock_export stock_Export)
        {
            TempData["barcode"] = stock_Export.Bar_code;
            if (stock_Export.Bar_code != null)
            {
                return RedirectToAction("ProcessDetails");
            }
            else {
                return View();
           }
        }
        public async Task<ActionResult> ProcessDetails()
        {
            string barcode = "";
            if (TempData.ContainsKey("barcode"))
            barcode = TempData["barcode"].ToString();
            
            BarcodeDetails bd = new BarcodeDetails();
            IList<Models.stock_export> stock_s = new List<Models.stock_export>();
            var productresult = await bd.GetDetails(barcode);
            if(productresult != null)
            {
                foreach (TestDetails.stock_export stock in productresult)
                {
                    Models.stock_export se = new Models.stock_export();
                    se.Bar_code = stock.Bar_code;
                    se.Stock_code = stock.Stock_code;
                    se.Stock_description = stock.Stock_description;
                    stock_s.Add(se);
                }
            }
            return View(stock_s);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}