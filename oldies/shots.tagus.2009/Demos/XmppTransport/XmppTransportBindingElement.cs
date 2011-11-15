using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;

namespace Xmpp
{
    public class XmppTransportBindingElement : TransportBindingElement
    {
        public XmppTransportBindingElement() { }
        public XmppTransportBindingElement(XmppTransportBindingElement be) { }
        public override BindingElement Clone()
        {
            return new XmppTransportBindingElement(this);
        }
        public override T GetProperty<T>(BindingContext context)
        {
            return context.GetInnerProperty<T>();
        }

        public override bool CanBuildChannelListener<TChannel>(BindingContext context)
        {
            return typeof(TChannel) == typeof(IInputChannel);
        }
        public override bool CanBuildChannelFactory<TChannel>(BindingContext context)
        {
            return typeof(TChannel) == typeof(IOutputChannel);
        }
        public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
        {
            XmppAccountBehavior bh = context.BindingParameters.Find<XmppAccountBehavior>();
            if (bh == null)
            {
                throw new Exception("Using the XmppTransportBindingElement requires a XmppAccountBehavior endpoint behavior with the account details");
            }
            MessageEncodingBindingElement mebe = context.BindingParameters.Find<MessageEncodingBindingElement>();
            if (mebe == null)
            {
                throw new Exception("Cannot find a MessageEncodingBindingElement");
            }


            return (IChannelFactory<TChannel>)(object)new XmppChannelFactory(bh,mebe.CreateMessageEncoderFactory());
        }
        public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context)
        {
            XmppAccountBehavior bh = context.BindingParameters.Find<XmppAccountBehavior>();
            if (bh == null)
            {
                throw new Exception("Using the XmppTransportBindingElement requires a XmppAccountBehavior endpoint behavior with the account details");
            }
            MessageEncodingBindingElement mebe = context.BindingParameters.Find<MessageEncodingBindingElement>();
            if (mebe == null)
            {
                throw new Exception("Cannot find a MessageEncodingBindingElement");
            }
            Uri uri = new Uri(context.ListenUriBaseAddress, context.ListenUriRelativeAddress);
            return (IChannelListener<TChannel>)(object)new XmppChannelListener(bh, mebe.CreateMessageEncoderFactory(), uri);
        }


        public override string Scheme
        {
            get { return "xmpp"; }
        }
    }
}
