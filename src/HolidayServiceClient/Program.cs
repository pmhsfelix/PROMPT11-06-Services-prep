using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HolidayServiceClient.HolidayServiceReference;

namespace HolidayServiceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new HolidayServiceReference.HolidayService2SoapClient("HolidayService2Soap12");
            
            var holidays = client.GetHolidaysForMonth(Country.Canada, 2011, 12);
            foreach (var holiday in holidays)
            {
                Console.WriteLine("{0} - {1}",holiday.Descriptor,holiday.Date);
            }

            var countries = client.GetCountriesAvailable();
            foreach (var country in countries)
            {
                Console.WriteLine("Country = {0}, code = {1}",country.Description,country.Code);
            }
        }
    }
}
