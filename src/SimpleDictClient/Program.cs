using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleDictClient.DictServiceReference;

namespace SimpleDictClient
{
    class Program
    {
        static void Main(string[] args)
        {

            var client = new DictServiceReference.TheServiceContractClient();
            var resp = client.WordLookup(new WordLookupReq() {Word = "professor"});
            Console.WriteLine(resp.Exists ? resp.Definition : "not found");
        }
    }
}
