using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using agsXMPP;
using System.Xml.Linq;
using System.Threading;

namespace Xmpp
{
    class MyAsyncResult : IAsyncResult
    {
        #region IAsyncResult Members

        object state;

        public MyAsyncResult(object state)
        {
            this.state = state;
        }

        public object AsyncState
        {
            get { return state; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get { throw new NotImplementedException(); }
        }

        public bool CompletedSynchronously
        {
            get { return true; }
        }

        public bool IsCompleted
        {
            get { return true; }
        }

        #endregion
    }

    class XmppChannelListener : ChannelListenerBase<IInputChannel>
    {


        public static void OnWriteXml(object sender, string xml)
        {
            Console.WriteLine("-- Listener: on write XML --");
            ShowXml(xml);
        }
        public static void OnReadXml(object sender, string xml)
        {
            Console.WriteLine("-- Listener: on read XML --");
            ShowXml(xml);
        }
        private static void ShowXml(string xml)
        {
            try
            {
                XElement xe = XElement.Parse(xml);
                Console.WriteLine(xe.ToString());
            }
            catch (Exception)
            {
                Console.WriteLine("!! Invalid XML:{0}", xml);
            }
        }

        private void OnMessage(object sender, agsXMPP.protocol.client.Message msg)
        {
            Console.WriteLine("-- Message received and enqueued --");
            Q.Enqueue(msg.Body);
        }

        MyQueue<string> Q = new MyQueue<string>();

        XmppAccountBehavior Account;
        MessageEncoderFactory mef;

        public readonly XmppClientConnection conn;

        System.Uri uri;

        public XmppChannelListener(XmppAccountBehavior bh, MessageEncoderFactory mef, System.Uri uri)
        {
            this.mef = mef;
            this.uri = uri;
            Account = bh;

            conn = new XmppClientConnection(Account.Server, Account.Port);
            conn.ConnectServer = Account.ConnectServer;

            conn.OnWriteXml += OnWriteXml;
            conn.OnReadXml += OnReadXml;

            conn.OnMessage += OnMessage;
        }

        protected override IInputChannel OnAcceptChannel(TimeSpan timeout)
        {
            throw new NotImplementedException();           

        }

        protected override IAsyncResult OnBeginAcceptChannel(TimeSpan timeout, AsyncCallback callback, object state)
        {
            //throw new NotImplementedException();
            Console.WriteLine("-- OnBeginAcceptChannel --");
            return Q.BeginDequeue(callback, state);
            
        }

        protected override IInputChannel OnEndAcceptChannel(IAsyncResult result)
        {
            //throw new NotImplementedException();
            string s = Q.EndDequeue(result);
            return new XmppInputChannel(this, mef.Encoder, s);
        }

        protected override IAsyncResult OnBeginWaitForChannel(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        protected override bool OnEndWaitForChannel(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        protected override bool OnWaitForChannel(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public override System.Uri Uri
        {
            get { return uri; }
        }



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
            conn.Open(Account.Credentials.UserName, Account.Credentials.Password);
            XmppConnectionState state = conn.XmppConnectionState;
            while (state != XmppConnectionState.SessionStarted)
            {
                if (conn.XmppConnectionState != state)
                {
                    state = conn.XmppConnectionState;
                    Console.WriteLine("Listener: state changed to {0}", state);
                }
                Thread.Sleep(100);
            }
            Console.WriteLine("Listener: connected");
            conn.SendMyPresence();
        }
    }

        
}
