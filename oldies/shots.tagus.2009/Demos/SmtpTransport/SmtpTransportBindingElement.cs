using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;

namespace Email
{
    public class EmailTransportBindingElement : TransportBindingElement
    {
        public EmailTransportBindingElement() { }
        public EmailTransportBindingElement(EmailTransportBindingElement be) { }
        public override BindingElement Clone()
        {
            return new EmailTransportBindingElement(this);
        }
        public override T GetProperty<T>(BindingContext context)
        {
            return context.GetInnerProperty<T>();
        }

        public override bool CanBuildChannelListener<TChannel>(BindingContext context)
        {
            return false;
        }
        public override bool CanBuildChannelFactory<TChannel>(BindingContext context)
        {
            return typeof(TChannel) == typeof(IOutputChannel);
        }
        public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
        {
            EmailAccountBehavior bh = context.BindingParameters.Find<EmailAccountBehavior>();
            if (bh == null)
            {
                throw new Exception("Using the EmailTransportBindingElement requires a EmailAccountBehavior endpoint behavior with the SMTP account details");
            }
            MessageEncodingBindingElement mebe = context.BindingParameters.Find<MessageEncodingBindingElement>();
            if (mebe == null)
            {
                throw new Exception("Cannot find a MessageEncodingBindingElement");
            }


            return (IChannelFactory<TChannel>)(object)new EmailChannelFactory(bh,mebe.CreateMessageEncoderFactory());
        }

        public override string Scheme
        {
            get { return "email"; }
        }
    }
}
