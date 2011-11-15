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

using System.Xml.Linq;
using System.ServiceModel.Channels;
using DictionaryService;
using System.ServiceModel.Description;
using System.Runtime.Remoting; // Only for the contracts sharing demo

namespace DictionaryClient {
    class ClientProgram {

        /*
         * Client using a Channel Factory directly and a shared 
         * service contract interface.
         * The binding and address are hard-coded
         */
        static string DictionaryLookup(string s) {

            var cf =
                new ChannelFactory<IDictionaryService>( // Uses the shared interface
                    new BasicHttpBinding(), // hard-coded binding
                    "http://localhost:8080/DictionaryService"); // hard-coded address

            IDictionaryService ch = cf.CreateChannel();
            IClientChannel ich = ch as IClientChannel;

            try {

                var req = new WordLookupRequestData();
                req.Word = s;
                var resp = ch.WordLookup(req);
                ich.Close();
                if (resp.Exists == false) {
                    return "Sorry, that word does not exists in the dictionary";
                }
                return resp.Definition;
            } catch (TimeoutException) {
                ich.Abort();
                throw;
            } catch (CommunicationException) {
                ich.Abort();
                throw;
            }
            
        }

        /*
         *  Client using a generic message contract.
         *  The binding and address are hard-coded
         */
        static string DictionaryLookup2(string s) {

            var cf = new ChannelFactory<IGenericRequestReplyContract>(
                    new BasicHttpBinding(),
                    "http://localhost:8080/DictionaryService");

            var ch = cf.CreateChannel();
            IClientChannel ich = ch as IClientChannel;

            // Create the request message body,
            // with the appropriate format
            XNamespace ns = "http://pedrofelix.local";
            XElement body = new XElement(ns + "WordLookup",
                                new XElement(ns + "request",
                                    new XElement(ns + "Word", s)
                            ));
            try {
                var responseMessage = ch.Operation(
                    Message.CreateMessage(
                        cf.Endpoint.Binding.MessageVersion, // SOAP version
                        "http://pedrofelix.local/DictionaryService/WorkLookup", // Message Action
                        body.CreateReader()
                    )
                );
                ich.Close();
                // Parse XML response 
                return responseMessage.GetBody<XElement>().Descendants(ns + "Definition").First().Value;
            } catch (TimeoutException) {
                ich.Abort();
                throw;
            } catch (CommunicationException) {
                ich.Abort();
                throw;
            }
        }

        /*
         *  Client using a svcutil.exe generated client class
         *  and configuration file
         */
        static string DictionaryLookup3(string s) {
            // Simply create the Client Object:
            //  - It implements the service interface
            //  - The binding and address are defined in the config file
            var client = new DictionaryClient.ServiceReference1.DictionaryServiceClient();
            try {
                var response = client.WordLookup(new DictionaryClient.ServiceReference1.WordLookupRequestData() { Word = s });
                if (!response.Exists) {
                    return "Sorry, that word does not exists in the dictionary";
                }
                client.Close();
                return response.Definition;
            } catch (TimeoutException) {
                client.Abort();
                throw;
            } catch (CommunicationException) {
                client.Abort();
                throw;
            }
        }

        /*
         *  Client dynamic metadata resolution and a shared contract
         */
        static string DictionaryLookup4(string s) {
            // Fetch the metadata (WSDL), using a HTTP GET, and obtain the
            // binding and the address from it
            var sec = MetadataResolver.Resolve(typeof(IDictionaryService), 
                new Uri("http://localhost:8080/metadata"), MetadataExchangeClientMode.HttpGet);
            ChannelFactory<IDictionaryService> cf =
                new ChannelFactory<IDictionaryService>(
                sec[0].Binding, // <- resolved binding
                sec[0].Address  // <- resolved address
                );

            IDictionaryService ch = cf.CreateChannel();
            IClientChannel ich = ch as IClientChannel;

            try {
                var response = ch.WordLookup(new WordLookupRequestData() { Word = s });
                if (!response.Exists) {
                    return "Sorry, that word does not exists in the dictionary";
                }
                ich.Close();
                return response.Definition;
            } catch (TimeoutException) {
                ich.Abort();
                throw;
            } catch (CommunicationException) {
                ich.Abort();
                throw;
            }
        }

        static void Main(string[] args) {
            Console.WriteLine("DictionaryLookup1:{0}", DictionaryLookup("professor"));
            Console.WriteLine("DictionaryLookup2:{0}", DictionaryLookup2("professor"));
            Console.WriteLine("DictionaryLookup3:{0}", DictionaryLookup3("professor"));
            Console.WriteLine("DictionaryLookup4:{0}", DictionaryLookup4("professor"));
        }
    }
}
