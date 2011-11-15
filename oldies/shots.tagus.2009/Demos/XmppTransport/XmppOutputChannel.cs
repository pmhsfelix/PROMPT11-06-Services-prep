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
using agsXMPP;

namespace Xmpp
{
    class XmppOutputChannel : ChannelBase, IOutputChannel
    {
        
        XmppChannelFactory fact;
        EndpointAddress epa;        
        MessageEncoder encoder;
        Jid remote;
        
        public XmppOutputChannel(XmppChannelFactory fact, EndpointAddress epa, MessageEncoder enc) : base(fact) {
            encoder = enc;
            this.epa = epa;
            if (epa.Uri.Scheme != "xmpp")
            {
                throw new InvalidOperationException("invalid address scheme. Must be 'xmpp'");
            }
            remote = new Jid(epa.Uri.AbsolutePath);
            this.fact = fact;            
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
            message.Headers.To = epa.Uri;
            MemoryStream ms = new MemoryStream();
            encoder.WriteMessage(message, ms);
            ms.Position = 0;
            TextReader tr = new StreamReader(ms);
            
            agsXMPP.protocol.client.Message msg = new agsXMPP.protocol.client.Message(
                remote,
                agsXMPP.protocol.client.MessageType.chat,
                tr.ReadToEnd()
            );
            
            fact.conn.Send(msg);
        }

        public System.Uri Via
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
