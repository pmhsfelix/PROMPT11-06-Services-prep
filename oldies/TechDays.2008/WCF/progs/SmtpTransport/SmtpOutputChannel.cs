using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.ServiceModel;

namespace Email
{
    class EmailOutputChannel : ChannelBase, IOutputChannel
    {
        SmtpClient client;
        EmailChannelFactory fact;
        EndpointAddress epa;
        string emailaddress;
        MessageEncoder encoder;

        public EmailOutputChannel(EmailChannelFactory fact, EndpointAddress epa, MessageEncoder enc) : base(fact) {
            encoder = enc;
            this.epa = epa;
            if (epa.Uri.Scheme != "mailto")
            {
                throw new InvalidOperationException("invalid address scheme. Must be 'mailto'");
            }
            emailaddress = epa.Uri.UserInfo + "@" + epa.Uri.Host;
            this.fact = fact;
            client = new SmtpClient(fact.Account.Host, fact.Account.Port);
            client.EnableSsl = fact.Account.UseSSL;
            client.Credentials = fact.Account.Credentials;
       }

        protected override void OnAbort()
        {
            
        }

        protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        protected override void OnClose(TimeSpan timeout)
        {
            //throw new NotImplementedException();
        }

        protected override void OnEndClose(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        protected override void OnEndOpen(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        protected override void OnOpen(TimeSpan timeout)
        {
            //throw new NotImplementedException();
        }


        #region IOutputChannel Members

        public IAsyncResult BeginSend(Message message, TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginSend(Message message, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public void EndSend(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public System.ServiceModel.EndpointAddress RemoteAddress
        {
            get { throw new NotImplementedException(); }
        }

        public void Send(Message message, TimeSpan timeout)
        {
            Send(message);
        }

        public void Send(Message message)
        {
            MailAddress from = new MailAddress(fact.Account.Credentials.UserName, "WCF email channel");
            MailAddress to = new MailAddress(emailaddress, epa.Uri.UserInfo);
            MailMessage mailmessage = new MailMessage(from, to);
            mailmessage.Subject = message.Headers.Action;
            MemoryStream ms = new MemoryStream();
            encoder.WriteMessage(message, ms);
            ms.Position = 0;
            TextReader tr = new StreamReader(ms);
            mailmessage.Body = tr.ReadToEnd();
            client.Send(mailmessage);
        }

        public Uri Via
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
