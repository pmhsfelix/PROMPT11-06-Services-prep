//-----------------------------------------------------------------------------
// WCF Samples and Demos, for educational purposes only
//
// Pedro Félix (pedrofelix at cc.isel.ipl.pt)
// Centro de Cálculo do Instituto Superior de Engenharia de Lisboa
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Xml.Linq;
using System.ServiceModel.Web;
using System.ServiceModel.Security;
using System.Security.Cryptography.X509Certificates;

namespace DictionaryService {

   
    // The service contract ...
    [ServiceContract(Namespace = "http://pedrofelix.local/", Name = "DictionaryService")]
    public interface IDictionaryService {
        [OperationContract(
            Action = "http://pedrofelix.local/DictionaryService/WorkLookup",
            ReplyAction = "http://pedrofelix.local/DictionaryService/WordLookupResponse")
        ]
        WordLookupResponseData WordLookup(WordLookupRequestData request);

    }

    // The Data Contracts ...
    // ... the request
    [DataContract(
        Namespace = "http://pedrofelix.local/",
        Name = "WordLookupRequestData"
        )]
    public class WordLookupRequestData {
        [DataMember]
        public string Word;
    }

    // ... the response
    [DataContract(
        Namespace = "http://pedrofelix.local/",
        Name = "WordLookupResponseData"
        )]
    public class WordLookupResponseData {
        [DataMember]
        public bool Exists;

        [DataMember]
        public string Definition;
    }
}