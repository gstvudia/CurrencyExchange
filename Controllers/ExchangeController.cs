using System;
using System.Xml;
using System.Xml.Linq;
using System.Web.Mvc;
using System.Text;
using System.Collections.Generic;
using CurrencyExchange.Models;
using CurrencyExchange.Services;
using System.Configuration;

namespace CurrencyExchange.Controllers
{
    public class ExchangeController : Controller
    {
        //Initialize the Context and the Service object
        CurrencyService service = new CurrencyService();

        private readonly DbInitializer _context;
        DbInitializer context = new DbInitializer();

        public ExchangeController()
        {
            _context = context;
        }
       

        //Simply return the home view
        #region
        public ActionResult Home()
        {

            return View();
        }
        #endregion

        //Gets a list of currencies names for the dropdown lists
        #region
        [HttpGet]
        public JsonResult GetCurrencies()
        {
            var Currencies = service.GetCurrencies();            
            return Json(Currencies, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //Calculate the exchange between the two currencies
        #region
        [HttpPost]
        public string CalculateExchange(string fromCurrency, string toCurrency, string value)
        {
            string rateResult = "";
            Decimal fromValue = 0, toValue = 0;
            var rate = service.GetRates(fromCurrency, toCurrency, true);

            if (fromCurrency != toCurrency)
            {
                foreach (Currency item in rate)
                {
                    if (item.Name == fromCurrency)
                    {
                        fromValue = item.Rate;
                    }
                    else
                        toValue = item.Rate;
                }

                //Apply the formula for the exchange
                fromValue = (Convert.ToDecimal(value) / fromValue) * toValue;
                rateResult = Math.Round(fromValue, 2).ToString();
            }
            else
            {
                rateResult = value;
            }
            
            return rateResult;
        }
        #endregion

        //Gets the data to be used in the chart
        #region
        [HttpPost]     
        public JsonResult GetChartData(string fromCurrency, string toCurrency)
        {
            List<ChartRequest> chartData = new List<ChartRequest>();
            List<Currency> rate = new  List<Currency>();
            ChartService serviceChart = new ChartService();

            rate = service.GetRates(fromCurrency, toCurrency, false);

            foreach (Currency item in rate)
            {
                item.Time = item.Time.ToUniversalTime();
                chartData.Add(serviceChart.ConvertTimeToUNIX(item));
            }

            return Json(chartData, JsonRequestBehavior.AllowGet);
        }
        #endregion  
    }
}