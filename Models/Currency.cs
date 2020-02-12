using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CurrencyExchange.Models
{
    public class Currency
    {

        public int ID { get; set; }
        public DateTime Time { get; set; }
        public string Name { get; set; }
        public Decimal Rate { get; set; }

    }
}
