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
using System.ServiceModel;

namespace DictionaryService {
    
    // The service implementation ...
    [ServiceBehavior(Name="DictionaryService", Namespace="http://pedrofelix.local/",IncludeExceptionDetailInFaults = true)]
    internal class DictionaryServiceImpl : IDictionaryService {

        static Dictionary<string, string> dictionary;
        static DictionaryServiceImpl() {
            dictionary = new Dictionary<string, string>();
            dictionary["professor"] = "Aquele que ensina";
        }

        public WordLookupResponseData WordLookup(WordLookupRequestData req) {
            WordLookupResponseData resp = new WordLookupResponseData();
            string def = null;
            if (dictionary.TryGetValue(req.Word, out def)) {
                resp.Exists = true;
                resp.Definition = def;
            } else {
                resp.Exists = false;
            }
            return resp;
        }       
    }
}
