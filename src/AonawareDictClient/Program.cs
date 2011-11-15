using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AonawareDictClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new DictServiceReference.DictServiceSoapClient("DictServiceSoap12");
            var resp = client.Define("professor");
            foreach (var def in resp.Definitions)
            {
                Console.WriteLine("{0}:{1}",def.Word, def.WordDefinition);
            }
        }
    }
}
