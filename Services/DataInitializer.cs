using System;
using System.Xml;
using System.Collections.Generic;
using CurrencyExchange.Models;
using System.Configuration;

namespace CurrencyExchange.Services
{
    public class DataInitializer
    {
        //Initialize the Context  
        private readonly DbInitializer _context;
        DbInitializer context = new DbInitializer();

        public DataInitializer()
        {
            _context = context;
        }

        public static void Init()
        {
            //Creating the Currency object and the list of Currencies
            int key = 1;
            Currency CurrencyItem = new Currency();
            List<string> Currencies = new List<string>();
            CurrencyService service = new CurrencyService();


            //Checks if the DB is empty
            Currencies = service.GetCurrencies();

            if (Currencies.Count == 0)
            {
                string URLString = ConfigurationManager.AppSettings["UrlXML"];
                XmlDocument XMLDoc = new XmlDocument();

                XMLDoc.Load(URLString);

                //Iterate through the XML
                foreach (XmlNode node in XMLDoc.DocumentElement)
                {
                    //Checks if the loop reached the CUBE node
                    if (node.Name == "Cube" && node.ChildNodes[0].Name == "Cube")
                    {
                        //Iterating through the main node
                        foreach (XmlNode mainNode in node.ChildNodes)
                        {
                            // Gets the time from the node
                            CurrencyItem.Time = Convert.ToDateTime(mainNode.Attributes[0].InnerText);

                            //Iterating through the items
                            foreach (XmlNode child in mainNode.ChildNodes)
                            {
                                // Gets the currency and the rate
                                CurrencyItem.Name = child.Attributes[0].InnerText;
                                CurrencyItem.Rate = Convert.ToDecimal(child.Attributes[1].InnerText);
                                CurrencyItem.ID = key;
                                key += 1;

                                //Currencies.Add(CurrencyItem);
                                service.AddCurrencyRate(CurrencyItem);
                            }
                        }

                    }
                }

                Currencies = service.GetCurrencies();
            }
            else
                Currencies = service.GetCurrencies();
        }
    }
}