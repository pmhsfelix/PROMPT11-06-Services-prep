//-----------------------------------------------------------------------------
// WCF Samples and Demos, for educational purposes only
//
// Pedro Félix (pedrofelix at cc.isel.ipl.pt)
// Centro de Cálculo do Instituto Superior de Engenharia de Lisboa
//-----------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Xml;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;
using System.ServiceModel.Web;
using System.ServiceModel.Description;
using System.Linq;
using System.Xml.Linq;
using Microsoft.ServiceBus;
using System.Security.Principal;
using System.ServiceModel.Security.Tokens;
using Email;
using Xmpp;

namespace Credentials
{
    //-
    // The data contract defining a message
    //-
    [DataContract(Namespace="http://pedrofelix.local")]
    public class MyMessage{
        [DataMember]
        public string Text;
    }

    //- 
    // The service contract defining the messaging handling
    //-
    [ServiceContract(Namespace = "http://pedrofelix.local")]
    public interface IMessaging
    {
        [OperationContract(IsOneWay = true, Action="http://pedrofelix.local/im/processmessage") ]
        void ProcessMessage(MyMessage s);     

    }

    class MessagingImpl : IMessaging
    {
        // Helper method to show the authenticater sender identity
        private void ShowSenderIdentity()
        {
            ServiceSecurityContext ssc = ServiceSecurityContext.Current;
            OperationContext oc = OperationContext.Current;
            
            if (ssc == null || !ssc.AuthorizationContext.Properties.ContainsKey("Identities"))
            {
                Console.WriteLine("Sender: Anonymous");
            }
            else
            {
                foreach(IIdentity ident in (ssc.AuthorizationContext.Properties["Identities"] as IList<IIdentity>)) {
                    Console.WriteLine("Sender: {0}, [{1}]", ident.Name, ident.AuthenticationType);
                }
            }
        }

        public void ProcessMessage(MyMessage m)
        {
            Console.WriteLine("Message received: {0}", m.Text);
            ShowSenderIdentity();
        }
        
    }

    class Program
    {

        public static Binding GetOneWaySignedBinding()
        {
            BindingElementCollection bec = new BindingElementCollection();

            // Add message signature
            AsymmetricSecurityBindingElement sbe = SecurityBindingElement.CreateCertificateSignatureBindingElement();            
            bec.Add(sbe);

            // HTTP 
            //bec.Add(new OneWayBindingElement());
            //bec.Add(new HttpTransportBindingElement());

            // Name pipe one way
            //bec.Add(new OneWayBindingElement());
            //bec.Add(new NamedPipeTransportBindingElement());

            // Service bus            
            bec.Add(new RelayedOnewayTransportBindingElement(RelayClientAuthenticationType.None, RelayedOnewayConnectionMode.Multicast));

            // XMPP
            //bec.Add(new TextMessageEncodingBindingElement());
            //bec.Add(new XmppTransportBindingElement());

            // SMTP
            //bec.Add(new TextMessageEncodingBindingElement());
            //bec.Add(new EmailTransportBindingElement());

            return new CustomBinding(bec);        
        }        
        
       
        //static string address = "http://gaviao:8080/ms";
        //static string address = "net.pipe://coruja/ms";
        //static string address = "mailto:pedrofelix@cc.isel.ipl.pt";
        static string address = "sb://felixdemos.servicebus.windows.net/oneway/ms";
        //static string viaAddress = "mailto:pedrofelix@cc.isel.ipl.pt";
        //static string address = "xmpp:xmpp2@sapo.pt";

        static void AcceptMessages()
        {
            ServiceHost sh = new ServiceHost(typeof(MessagingImpl));
            ServiceEndpoint sep = sh.AddServiceEndpoint(typeof(IMessaging), GetOneWaySignedBinding(), address);
            sep.Behaviors.Add(new ServiceRegistrySettings() {
                DiscoveryMode = DiscoveryType.Private,
                DisplayName = "Messaging Service"             
            });
            CredentialsHelper.ConfigureServiceCredentials(sh);
            sh.Open();
            Console.WriteLine("The service host is listening at:\n {0} \n press any key to close",address);            
        }

        static void SendMessage(string s)
        {
            ChannelFactory<IMessaging> cf = new ChannelFactory<IMessaging>(GetOneWaySignedBinding());
            CredentialsHelper.ConfigureClientCredentials(cf);
            //IMessaging ch = cf.CreateChannel(new EndpointAddress(address),new Uri(viaAddress));
            IMessaging ch = cf.CreateChannel(new EndpointAddress(address));
            IClientChannel ich = ch as IClientChannel;
            try
            {
                ch.ProcessMessage(new MyMessage() { Text = s });
                Console.WriteLine("Message sent");
                ich.Close();

            }
            catch (CommunicationException)
            {
                ich.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                ich.Abort();
                throw;
            }
        }

        public static void Main(string[] args)
        {
            AcceptMessages();
            AcceptMessages();
            SendMessage("hello");

            Console.ReadKey();
        }
    }
}
