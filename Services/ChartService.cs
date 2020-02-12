using System;
using CurrencyExchange.Models;

namespace CurrencyExchange.Services
{
    public class ChartService
    {
        //Convert the Time to UNIX
        #region
        public ChartRequest ConvertTimeToUNIX(Currency currency)
        {
            using (var context = new DbInitializer())
            {
                //Converts the time value to UNIX

                DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                long unixTimeStampInTicks = (currency.Time.ToUniversalTime() - unixStart).Ticks;

                var unix = Convert.ToInt32((double)unixTimeStampInTicks / TimeSpan.TicksPerSecond);

                ChartRequest chartData = new ChartRequest
                {
                    Time = unix,
                    Rate = currency.Rate,
                    Name = currency.Name

                };

                return chartData;
            }

        }
        #endregion
    }
}
