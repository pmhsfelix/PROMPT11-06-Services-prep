using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;

namespace Email
{
    class EmailChannelFactory : ChannelFactoryBase<IOutputChannel>
    {
        public readonly EmailAccountBehavior Account;
        MessageEncoderFactory mef;
        public EmailChannelFactory(EmailAccountBehavior bh, MessageEncoderFactory mef)
        {
            this.mef = mef;
            Account = bh;
        }
        protected override IOutputChannel OnCreateChannel(System.ServiceModel.EndpointAddress address, Uri via)
        {
            return new EmailOutputChannel(this, address, via, mef.Encoder);
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
            
        }
    }
}
