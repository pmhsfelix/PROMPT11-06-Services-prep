//-----------------------------------------------------------------------------
// WCF Samples and Demos, for educational purposes only
//
// Pedro Félix (pedrofelix at cc.isel.ipl.pt)
// Centro de Cálculo do Instituto Superior de Engenharia de Lisboa
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Use the spellingSuggestion Google service
// via a typed client object (GoogleService.GoogleSearchPortClient())
namespace GoogleClient {
    class Program {
        // service key: use it. Please, don't abuse it.
        const string key = "yoH9tetQFHJ5At7by44uCn9+ip+s0nD9";
        static void Main(string[] args) {
            var client = new GoogleClient.ServiceReference1.GoogleSearchPortClient();

            Console.WriteLine(client.doSpellingSuggestion(key,"Lissboa"));
        }
    }
}
