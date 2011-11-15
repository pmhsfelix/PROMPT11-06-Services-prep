using System;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text;

public sealed class DelegatorBinding : Binding {
    
    String _scheme;
    BindingElementCollection _elements;

    public DelegatorBinding(BindingMode mode) : this(mode, 0, null) {
    
    }

    public DelegatorBinding(BindingMode bindingMode, Int32 elementPosition, Uri clientBaseAddress) {

        if ((clientBaseAddress == null) && (bindingMode == BindingMode.WSDualHttp)) {
            throw new ArgumentNullException("ClientBaseAddress cannot be null with WsDual Binding Mode", "clientBaseAddress");
        }
        
        switch (bindingMode) {
            case (BindingMode.BasicHttp):
                BasicHttpBinding httpBinding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                _elements = httpBinding.CreateBindingElements();
                _scheme = "http";
                break;
            case (BindingMode.Tcp):
                _elements = new NetTcpBinding(SecurityMode.None, false).CreateBindingElements();
                _scheme = "net.tcp";
                break;
            case (BindingMode.TcpRM):
                _elements = new NetTcpBinding(SecurityMode.None, true).CreateBindingElements();
                _scheme = "net.tcp";
                break;
            case (BindingMode.WSHttp):
                _elements = new WSHttpBinding(SecurityMode.None, false).CreateBindingElements();
                _scheme = "http";
                break;
            case (BindingMode.WSHttpRM):
                _elements = new WSHttpBinding(SecurityMode.None, true).CreateBindingElements();
                _scheme = "http";
                break;
            case (BindingMode.WSDualHttp):
                WSDualHttpBinding wsBinding = new WSDualHttpBinding(WSDualHttpSecurityMode.None);
                wsBinding.ClientBaseAddress = clientBaseAddress;
                _elements = wsBinding.CreateBindingElements();
                _scheme = "http";
                break;
            case (BindingMode.MSMQ):
                NetMsmqBinding msmqBinding = new NetMsmqBinding(NetMsmqSecurityMode.None);
                msmqBinding.ExactlyOnce = false;
                _elements = msmqBinding.CreateBindingElements();
                _scheme = "net.msmq";
                break;
            case (BindingMode.MSMQSession):
                NetMsmqBinding msmqTransactionalBinding = new NetMsmqBinding(NetMsmqSecurityMode.None);
                msmqTransactionalBinding.ExactlyOnce = true;
                _elements = msmqTransactionalBinding.CreateBindingElements();
                _elements[0] = new TextMessageEncodingBindingElement(MessageVersion.Default, Encoding.UTF8);
                _scheme = "net.msmq";
                break;
            default:
                throw new ArgumentOutOfRangeException("bindingMode");
        }

        // add the DelegatorBindingElement
        _elements.Insert(elementPosition, new DelegatorBindingElement());
    }

    public override BindingElementCollection CreateBindingElements() { 
        return _elements;
    }

    public override string Scheme {
        get {
            return _scheme;
        }
    }

}

public enum BindingMode {
    Tcp,
    TcpRM,
    WSHttp,
    WSHttpRM,
    WSDualHttp,
    BasicHttp,
    PeerChannel,
    MSMQ,
    MSMQSession
}
