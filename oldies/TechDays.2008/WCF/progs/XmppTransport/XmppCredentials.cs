using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using System.Net;
using System.ServiceModel;

namespace Xmpp
{
    public static class Extensions
    {
        public static void AddXmppAccountBehavior<T>(this ChannelFactory<T> cf, XmppAccountBehavior bh)
        {
            cf.Endpoint.Behaviors.Add(bh);
        }
    }
    public class XmppAccountBehavior : IEndpointBehavior
    {

        public string Server {get; set;}
        public int Port { get; set; }
        public string ConnectServer { get; set; }
        public readonly NetworkCredential Credentials = new NetworkCredential();
                
        #region IEndpointBehavior Members


        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            bindingParameters.Add(this);
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            //throw new NotImplementedException();
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            //throw new NotImplementedException();
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
