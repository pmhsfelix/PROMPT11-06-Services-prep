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

namespace DictionaryService
{
    
    class HostProgram
    {
        static void RunCodeBasedHost()
        {
            ServiceHost host = new ServiceHost(typeof(DictionaryServiceImpl));
            Binding b = new BasicHttpBinding();
            b.Namespace = "http://pedrofelix.local/";
            ServiceEndpoint ep = host.AddServiceEndpoint(
                typeof(IDictionaryService), 
                b, 
                "http://localhost:8081/DictionaryService");

                                    
            #region Acrescentar um behavior para a geração de metadata (WSDL)
            host.Description.Behaviors.Add(
                new ServiceMetadataBehavior()
                {
                    HttpGetEnabled = true,
                    HttpGetUrl = new Uri("http://localhost:8081/metadata")
                }
                );            
            #endregion

                      
            host.Open();
            Console.WriteLine("Dictionary service is now opened, press any key to close it");
          
            Console.ReadKey();
            host.Close();
        }

        static void RunConfigBasedHost() {
            ServiceHost host = new ServiceHost(typeof(DictionaryServiceImpl));
            
            host.Open();
            Console.WriteLine("Dictionary service is now opened, press any key to close it");

            Console.ReadKey();
            host.Close();
        }

        public static void Main(String[] args) {
            RunCodeBasedHost();
            //RunConfigBasedHost();
        }
    }
}
