using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using agsXMPP;
using System.Threading;
using System.Xml.Linq;

namespace Xmpp
{
    class XmppChannelFactory : ChannelFactoryBase<IOutputChannel>
    {

        public static void OnWriteXml(object sender, string xml)
        {
            Console.WriteLine("-- Factory: on write XML --");
            ShowXml(xml);
        }
        public static void OnReadXml(object sender, string xml)
        {
            Console.WriteLine("-- Factory: on read XML --");
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

        public readonly XmppAccountBehavior Account;
        MessageEncoderFactory mef;

        public readonly XmppClientConnection conn;

        public XmppChannelFactory(XmppAccountBehavior bh, MessageEncoderFactory mef)
        {
            this.mef = mef;
            Account = bh;

            conn = new XmppClientConnection(Account.Server, Account.Port);
            conn.ConnectServer = Account.ConnectServer;

            conn.OnWriteXml += OnWriteXml;
            conn.OnReadXml += OnReadXml;

        }
        protected override IOutputChannel OnCreateChannel(System.ServiceModel.EndpointAddress address, System.Uri via)
        {
            return new XmppOutputChannel(this, address, mef.Encoder);
        }

        protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        protected override void OnEndOpen(IAsyncResult result)
        {
            
        }

        protected override void OnOpen(TimeSpan timeout)
        {
            conn.Open(Account.Credentials.UserName,Account.Credentials.Password);
            XmppConnectionState state = conn.XmppConnectionState;
            while (state != XmppConnectionState.SessionStarted)
            {
                if (conn.XmppConnectionState != state)
                {
                    state = conn.XmppConnectionState;
                    Console.WriteLine("Factory: state changed to {0}", state);
                }
                Thread.Sleep(100);
            }
            Console.WriteLine("Factory: connected");
            conn.SendMyPresence();
        }
    }
}
