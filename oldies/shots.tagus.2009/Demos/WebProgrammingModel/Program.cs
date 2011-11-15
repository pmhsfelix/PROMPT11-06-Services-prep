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
using System.ServiceModel.Web;
using System.ServiceModel.Syndication;
using System.Diagnostics;
using System.ServiceModel;
using Microsoft.ServiceBus;
using Credentials;

namespace WebProgrammingModel {

    // The Service Contract ...
    [ServiceContract]
    public interface WebContract {
        [OperationContract]
        [WebGet(UriTemplate = "/gp/{name}")]
        Rss20FeedFormatter GetProcessFeed(string name);

    }

    // The Contract Implementation
    class WebContractImpl : WebContract {
        #region WebContract Members

        public Rss20FeedFormatter GetProcessFeed(string name) {

            return new Rss20FeedFormatter(
                new SyndicationFeed("Processes", "Processes with given image name", new Uri("http://no.where"),
                    Process.GetProcessesByName(name).Select(
                        proc => new SyndicationItem(String.Format("Process {0}", proc.Id),
                            String.Format("Process with image : {0}", proc.MainModule.FileName),
                            new Uri("http://msdn.microsoft.com/en-us/library/system.diagnostics.process.aspx"))
                    )
                ) 
            );
        }

        #endregion
    }

    class Program {

        static Uri GetServiceBusUri(){            
            return ServiceBusEnvironment.CreateServiceUri("http", Credentials.ServiceBusCredentials.SolutionaName, "wpm");            
        }

        static void SetServiceBusCredentials(ServiceHost sh){

            TransportClientEndpointBehavior tcred = new TransportClientEndpointBehavior();
            tcred.CredentialType = TransportClientCredentialType.SharedSecret;
            tcred.Credentials.SharedSecret.IssuerName = Credentials.ServiceBusCredentials.IssuerName;
            tcred.Credentials.SharedSecret.IssuerSecret = Credentials.ServiceBusCredentials.IssuerSecret;

            foreach (var sep in sh.Description.Endpoints) {
                sep.Behaviors.Add(tcred);
            }
        }

        static void Main(string[] args) {
            Uri sburi = GetServiceBusUri();
            var sh = new WebServiceHost(typeof(WebContractImpl));
            
            //sh.AddServiceEndpoint(typeof(WebContract), new WebHttpBinding(), "http://localhost/wpm/");
            sh.AddServiceEndpoint(typeof(WebContract), new WebHttpRelayBinding(EndToEndWebHttpSecurityMode.None, RelayClientAuthenticationType.None),sburi);
            SetServiceBusCredentials(sh);
            sh.Open();

            Console.WriteLine("Service opened, press any key to continue");
            Console.WriteLine("Cloud address is : \n{0}", sburi.ToString());
            Console.ReadKey();

            sh.Close();
        }
    }
}
