using System;
using CurrencyExchange.Models;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyExchange.Services
{
    public class CurrencyService
    {
        //Add a new currency & rate to the database
        #region
        public void AddCurrencyRate(Currency currency)
        {
            using (var context = new DbInitializer())

                try
                {
                    var check = context.Currencies.Contains(currency);
                    if (check == false)
                    {
                        context.Currencies.Add(currency);
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.ToString() == "An item with the same key has already been added.")
                    {
                        context.Currencies.Add(currency);
                        context.SaveChanges();
                    }
                }

        }
        #endregion


        //Get the latest currency in the database
        #region
        public List<string> GetCurrencies()
        {
            using (var context = new DbInitializer())
            {
                var listCurrencies = context.Currencies.Distinct().OrderByDescending(c => c.Time).ToList();
                List<string> result = new List<string>();

                if (listCurrencies.Count > 0)
                {
                    var date = listCurrencies.Max(d => d.Time);
                    var listCurrenciesNames = from
                                              cur in listCurrencies
                                              where
                                              (cur.Time.Day == date.Day && cur.Time.Month == date.Month)
                                              select
                                              cur.Name;

                    result =  listCurrenciesNames.ToList();
                }
                return result;
            }

        }
        #endregion


        //This method search for the rates of the currencies desired
        #region
        public List<Currency> GetRates(string fromCurrency, string toCurrency, Boolean onlyNames)
        {

            using (var context = new DbInitializer())
            {

                var listCurrencies = context.Currencies.OrderBy(t => t.Time).ToList();
                DateTime date = DateTime.Now;
                List<Currency> listRates = new List<Currency>();

                if (onlyNames == true)
                {
                     date = listCurrencies.Max(d => d.Time);
                

                    //Search in the list for the currencies and the latest
                    listRates = context.Currencies.OrderBy(t => t.Time)
                               .Where(c => c.Time.Day == date.Day && c.Time.Month == date.Month &&
                                      (c.Name == fromCurrency || c.Name == toCurrency)).ToList();
                }
                else
                    //Search in the list for the All currencies and the times
                    listRates = context.Currencies.OrderBy(t => t.Time)
                               .Where(c =>c.Name == fromCurrency || c.Name == toCurrency).ToList();
                return listRates;
            }
        }
        #endregion
    }
}
