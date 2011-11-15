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
using Microsoft.ServiceBus;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;
using System.ServiceModel;

using Xmpp;
using Email;

namespace Credentials {

    // Never do this in production!
    // Just for demoing purposes

    public sealed class ServiceBusCredentials {
        public const string SolutionaName = "felixdemos";
        public const string IssuerName = "owner";
        public const string IssuerSecret = "5OrzYPGnyl4unHfynb1M8owlRreLTNtznbzg1aEqLG8=";
    };

    public sealed class XmppCredentials {
        public const string UserName = "xmpp1";
        public const string Password = "changeit";
        public const string Server = "sapo.pt";
        public const string ConnectServer = "clientes.im.sapo.pt";
        public const int Port = 5222;
    }

    public sealed class SmtpCredentials {
        public const string UserName = "transportchannel@gmail.com";
        public const string Password = "changeit";
        public const string Host = "smtp.gmail.com";
        public const int Port = 587;
    }

    public sealed class X509Credentials {
        public const string ClientCertSubject = "Alice";
        public const string ServerCertSubject = "gaviao";
    }


    public static class CredentialsHelper {

        public static void ConfigureClientCredentials<T>(ChannelFactory<T> cf) {
            // Message level client credential (X.509 certificate)
            cf.Credentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindBySubjectName, X509Credentials.ClientCertSubject);
            // Message level client credential authentication settings
            cf.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.PeerOrChainTrust;
            cf.Credentials.ServiceCertificate.Authentication.TrustedStoreLocation = StoreLocation.CurrentUser;
            cf.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            // Transport level settings (for the Service Bus)
            TransportClientEndpointBehavior tcred = new TransportClientEndpointBehavior();
            tcred.CredentialType = TransportClientCredentialType.Unauthenticated;
            cf.Endpoint.Behaviors.Add(tcred);

            // Transport level settings (for SMTP)
            EmailAccountBehavior ebh = new EmailAccountBehavior()
            {
                Host = SmtpCredentials.Host,
                Port = SmtpCredentials.Port,
                UseSSL = true
            };
            ebh.Credentials.UserName = SmtpCredentials.UserName;
            ebh.Credentials.Password = SmtpCredentials.Password;
            ebh.Credentials.Domain = "";
            cf.AddEmailAccountBehavior(ebh);

            XmppAccountBehavior bh = new XmppAccountBehavior()
            {
                Server = XmppCredentials.Server,
                ConnectServer = XmppCredentials.ConnectServer,
                Port = XmppCredentials.Port,
            };
            bh.Credentials.UserName = XmppCredentials.UserName;
            bh.Credentials.Password = XmppCredentials.Password;
            bh.Credentials.Domain = "";

            //cf.Endpoint.Behaviors.Add(bh);
        }

        public static void ConfigureServiceCredentials(ServiceHost sh) {
            // Message level service credentials (X.509 certificate)
            sh.Credentials.ServiceCertificate.SetCertificate(StoreLocation.CurrentUser,StoreName.My,X509FindType.FindBySubjectName,X509Credentials.ServerCertSubject);                
                
            // Message level service credential authentication settings
            sh.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.PeerOrChainTrust;
            sh.Credentials.ClientCertificate.Authentication.TrustedStoreLocation = StoreLocation.CurrentUser;
            sh.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            // Transport level service credentials (for the Service Bus)
            TransportClientEndpointBehavior tcred = new TransportClientEndpointBehavior();
            tcred.CredentialType = TransportClientCredentialType.SharedSecret;
            tcred.Credentials.SharedSecret.IssuerName = ServiceBusCredentials.IssuerName;
            tcred.Credentials.SharedSecret.IssuerSecret = ServiceBusCredentials.IssuerSecret;

            foreach (var sep in sh.Description.Endpoints) {
                sep.Behaviors.Add(tcred);
            }

            // Transport level settings (for XMPP)
            XmppAccountBehavior bh = new XmppAccountBehavior()
            {
                Server = XmppCredentials.Server,
                ConnectServer = XmppCredentials.ConnectServer,
                Port = XmppCredentials.Port,
            };
            bh.Credentials.UserName = XmppCredentials.UserName;
            bh.Credentials.Password = XmppCredentials.Password;
            bh.Credentials.Domain = "";
            foreach (var sep in sh.Description.Endpoints) {
                sep.Behaviors.Add(bh);
            }
        }
    }
}
