using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using agsXMPP;
using System.IO;
using System.Threading;
using System.ServiceModel;

namespace Xmpp
{
    class XmppInputChannel : ChannelBase, IInputChannel
    {
        XmppChannelListener list;
        MessageEncoder encoder;
        string body;
        bool done = false;
       
        public XmppInputChannel(XmppChannelListener list, MessageEncoder enc, string body):base(list)
        {
            this.list = list;
            encoder = enc;
            this.body = body;
        }

        #region IInputChannel Members

        public IAsyncResult BeginReceive(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }
        public IAsyncResult BeginReceive(AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }
        public IAsyncResult BeginTryReceive(TimeSpan timeout, AsyncCallback callback, object state)
        {
            IAsyncResult ar = new CompletedSynchronouslyResult(state);
            if(callback != null){
                callback(ar);
            }
            return ar;
        }
        public IAsyncResult BeginWaitForMessage(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }
        public Message EndReceive(IAsyncResult result)
        {
            throw new NotImplementedException();
        }
        public bool EndTryReceive(IAsyncResult result, out Message message)
        {
            if (done)
                throw new CommunicationObjectAbortedException();
            message = Receive();
            done = true;
            return true;
        }
        public bool EndWaitForMessage(IAsyncResult result)
        {
            throw new NotImplementedException();
        }
        public System.ServiceModel.EndpointAddress LocalAddress
        {
            get { throw new NotImplementedException(); }
        }
        public Message Receive(TimeSpan timeout)
        {
            return Receive();
        }

        public Message Receive()
        {
            MemoryStream ms = new MemoryStream();
            TextWriter tw = new StreamWriter(ms);
            tw.Write(body);
            tw.Flush();
            ms.Position = 0;
            base.Close();
            Message msg = null;
            try
            {
                msg = encoder.ReadMessage(ms, 64*1024); // todo: change number           
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
            return msg;
        }

        public bool TryReceive(TimeSpan timeout, out Message message)
        {
            throw new NotImplementedException();
        }

        public bool WaitForMessage(TimeSpan timeout)
        {
            return true;
        }

        #endregion

        protected override void OnAbort()
        {
            //throw new NotImplementedException();
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
    }
}
