using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Security.Principal;
using System.ServiceModel.Description;
using Email;
using System.Windows.Forms;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using System.IdentityModel.Claims;

namespace Xmpp
{
    
    //-- The service's contract    
    [ServiceContract]
    public interface IMyService
    {
        [OperationContract(IsOneWay=true)]
        void Send(string s);
    }

    //-- The service's implementation
    class MyServiceImpl : IMyService
    {

        #region IMyService Members

        public void Send(string s)
        {
            MessageBox.Show(string.Format("{0} \n Sender: {1}",s,GetSenderIdentityInfo()),"Message received");
        }

        #endregion

        #region Utilities

        private string GetSenderIdentityInfo()
        {
            ServiceSecurityContext ssc = ServiceSecurityContext.Current;

            if (ssc != null)
            {

                // Check if there is a Primary Identity
                if (ssc.PrimaryIdentity != null && ssc.PrimaryIdentity.IsAuthenticated == true)
                {
                    return string.Format("name = {0}, authentication type = {1}", ssc.PrimaryIdentity.Name, ssc.PrimaryIdentity.AuthenticationType);
                }

                // Else, check if there is a self issued claim
                if (ssc.AuthorizationContext.ClaimSets != null)
                {

                    foreach (ClaimSet cs in ssc.AuthorizationContext.ClaimSets)
                    {
                        foreach (Claim c in cs)
                        {
                            if (c.ClaimType == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")
                            {
                                return string.Format("name = {0}, authentication type = Personal Information Card", c.Resource);
                            }
                        }
                    }
                }
            }

            // Otherwise, return anonymous
            return "anonymous";
        }

        #endregion
    }


    class Program
    {
        
        private static void SetClientCredentials<T>(ChannelFactory<T> cf)
        {
            XmppAccountBehavior bh = new XmppAccountBehavior()
            {
                Server = "sapo.pt",
                ConnectServer = "clientes.im.sapo.pt",
                Port = 5222
            };
            bh.Credentials.UserName = "xmpp1";
            bh.Credentials.Password = Credentials.Password;
            bh.Credentials.Domain = "";
            cf.AddXmppAccountBehavior(bh);

            EmailAccountBehavior ebh = new EmailAccountBehavior()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                UseSSL = true
            };
            ebh.Credentials.UserName = "transportchannel@gmail.com";
            ebh.Credentials.Password = Credentials.Password;
            ebh.Credentials.Domain = "";
            cf.AddEmailAccountBehavior(ebh);

            cf.Credentials.ClientCertificate.SetCertificate("cn=Alice", StoreLocation.CurrentUser, StoreName.My);
            cf.Credentials.ServiceCertificate.SetDefaultCertificate("cn=coruja", StoreLocation.CurrentUser, StoreName.My);
        }

        private static void SetEndpointCredentials(ServiceEndpoint sep)
        {
            XmppAccountBehavior bh = new XmppAccountBehavior()
            {
                Server = "sapo.pt",
                ConnectServer = "clientes.im.sapo.pt",
                Port = 5222
            };
            bh.Credentials.UserName = "xmpp1";
            bh.Credentials.Password = "changeit";
            bh.Credentials.Domain = "";

            sep.Behaviors.Add(bh);
        }

        private static void SetHostCredentials(ServiceHost sh)
        {
            sh.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.PeerOrChainTrust;
            sh.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            sh.Credentials.ClientCertificate.Authentication.TrustedStoreLocation = StoreLocation.CurrentUser;

            sh.Credentials.ServiceCertificate.SetCertificate("cn=coruja", StoreLocation.CurrentUser, StoreName.My);

            sh.Credentials.IssuedTokenAuthentication.AllowUntrustedRsaIssuers = true;
        }


        // Starts the service
        private static void StartService(string address)
        {
            ServiceHost sh = new ServiceHost(typeof(MyServiceImpl));
            SetHostCredentials(sh);
            ServiceEndpoint sep = sh.AddServiceEndpoint(typeof(IMyService), GetBinding(), address);
            SetEndpointCredentials(sep);
            sh.Open();
            Console.WriteLine("-- ServiceHost is opened "); 
        }

        // Runs the client
        private static void RunClient(Uri uri)
        {
            ChannelFactory<IMyService> cf = new ChannelFactory<IMyService>(GetBinding());
            SetClientCredentials(cf);            
            IMyService ch = cf.CreateChannel(
                new EndpointAddress(uri, 
                    EndpointIdentity.CreateX509CertificateIdentity(cf.Credentials.ServiceCertificate.DefaultCertificate)
                )
            );
            ch.Send("Olá, bem vindos à Curia");
            Console.WriteLine("-- Client ended");
        }

        // Creates and configures a binding element for self-issued tokens
        private static System.ServiceModel.Channels.BindingElement GetSelfIssuedForCertificateBindingElement()
        {
            IssuedSecurityTokenParameters itp = new IssuedSecurityTokenParameters();
            itp.ClaimTypeRequirements.Add(
                new ClaimTypeRequirement("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")
            );
            itp.IssuerAddress = new EndpointAddress("http://schemas.xmlsoap.org/ws/2005/05/identity/issuer/self");
            return SecurityBindingElement.CreateIssuedTokenForCertificateBindingElement(itp);
        }

        // Returns the binding used in the sample
        private static System.ServiceModel.Channels.Binding GetBinding()
        {
            //return new BasicHttpBinding();

            return new CustomBinding(

                //SecurityBindingElement.CreateCertificateSignatureBindingElement(),
                //SecurityBindingElement.CreateMutualCertificateBindingElement(),
                //GetSelfIssuedForCertificateBindingElement(),          

                new TextMessageEncodingBindingElement(),

                //new XmppTransportBindingElement()
                new EmailTransportBindingElement()

                //new OneWayBindingElement(),
                //new HttpTransportBindingElement()
            );
        }

        

        static void Main(string[] args)
        {
            //string endpointAddress = "http://coruja:8080/service/ep1";
            //string endpointAddress = "xmpp:xmpp1@sapo.pt";
            string endpointAddress = "mailto:pedrofelix@cc.isel.ipl.pt";
            
            //StartService(endpointAddress);
            RunClient(new Uri(endpointAddress));

            Console.WriteLine("-- Press any key to end");
            Console.ReadKey();
        }
    }


























    // NEVER do this, only for demo purposes
    class Credentials
    {
        public static string Password{ get { return "changeit";}}
    }
}
