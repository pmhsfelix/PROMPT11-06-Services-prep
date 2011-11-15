using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Transactions;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

internal sealed class Program
{
    static void Main()
    {
        //// datagram demos
        NetTcpBindingOneWayDemo();
        BasicHttpBindingOneWayDemo();
        WsHttpBindingOneWayDemo();
        NetMsmqBindingOneWayDemo();

        //// request / reply demos
        NetTcpBindingRequestReplyDemo();
        BasicHttpBindingRequestReplyDemo();
        WsHttpBindingRequestReplyDemo();

        // duplex demos
        NetTcpBindingDuplexDemo();

        // sessionful demo
        NetTcpBindingOneWaySessionDemo();
        // requires a private queue named "DelegatorSessionDemo"
        NetMsmqBindingSessionDemo();

        Console.ReadLine();
              
    }

    private static void NetMsmqBindingSessionDemo() {
        PrintHelper.Print("==== BEGIN One Way NetMSMQBinding Sessionful Demo ====");

        Uri address = new Uri("net.msmq://localhost/private/DelegatorSessionDemo");
        DelegatorBinding binding = new DelegatorBinding(BindingMode.MSMQSession);

        using (ServiceHost svc = new ServiceHost(typeof(OneWaySessionfulTransactionalReceiver))) {
            svc.AddServiceEndpoint(typeof(ISomeContractOneWaySession), binding, address);
            svc.Open();

            ReceiverReadyMessage();

            using (TransactionScope scope = new TransactionScope()) {
                using (MyOneWaySessionfulProxy proxy = new MyOneWaySessionfulProxy(binding, new EndpointAddress(address))) {
                    try {
                        proxy.SomeOperation("Hi there");
                        proxy.SomeOtherOperation("Hi There Again");
                    }
                    catch (InvalidOperationException ex) {
                        PrintHelper.Print(String.Format("{0}: {1}", ex.GetType().Name, ex.Message));
                    }
                    scope.Complete();
                }
            }

            // Give the receiver some time
            Thread.Sleep(5000);
        }

        PrintHelper.Print("==== END One Way NetMSMQBinding Sessionful Demo ====");
        PrintHelper.PrintNewLine();



    }

    private static void NetMsmqBindingOneWayDemo() {
        PrintHelper.Print("==== BEGIN One Way NetMsmqBinding Demo ====");

        Uri address = new Uri("net.msmq://localhost/private/delegatordemo");
        DelegatorBinding binding = new DelegatorBinding(BindingMode.MSMQ);

        using(ServiceHost svc = new ServiceHost(typeof(OneWayReceiver))) {
            svc.AddServiceEndpoint(typeof(ISomeContractOneWay), binding, address);
            svc.Open();

            ReceiverReadyMessage();

            using (MyOneWayProxy proxy = new MyOneWayProxy(binding, new EndpointAddress(address))) {
                while (Console.ReadLine() != "QUIT") {
                        proxy.SomeOperation("Hi there");
                }
            }

        }

        PrintHelper.Print("==== END One Way NetMsmqBinding Demo ====");
        PrintHelper.PrintNewLine();
    }

    private static void NetTcpBindingOneWaySessionDemo() {
        PrintHelper.Print("==== BEGIN One Way NetTcpBinding Sessionful Demo ====");

        Uri address = new Uri("net.tcp://localhost:4000/ISomeContractOneWaySessionful");
        DelegatorBinding binding = new DelegatorBinding(BindingMode.Tcp);

        using (ServiceHost svc = new ServiceHost(typeof(OneWaySessionfulReceiver))) {
            svc.AddServiceEndpoint(typeof(ISomeContractOneWaySession), binding, address);
            svc.Open();

            ReceiverReadyMessage();

            for (Int32 i = 0; i < 2; i++) {
                using (MyOneWaySessionfulProxy proxy = new MyOneWaySessionfulProxy(binding, new EndpointAddress(address))) {
                    while (Console.ReadLine() != "QUIT") {
                        try {
                            proxy.SomeOperation("Hi there");
                            proxy.SomeOtherOperation("Hi There Again");
                        }
                        catch (InvalidOperationException ex) {
                            PrintHelper.Print(String.Format("{0}: {1}", ex.GetType().Name, ex.Message));
                        }
                    }
                }
            }
        }

        PrintHelper.Print("==== END One Way NetTcpBinding Sessionful Demo ====");
        PrintHelper.PrintNewLine();
    }

    private static void NetTcpBindingDuplexDemo() {
        
        PrintHelper.Print("==== BEGIN Duplex NetTcpBinding Demo ====");
        
        Uri address = new Uri("net.tcp://localhost:4000/ISomeContractDuplex");
        DelegatorBinding binding = new DelegatorBinding(BindingMode.Tcp);

        using (ServiceHost svc = new ServiceHost(typeof(DuplexReceiver))) {
            svc.AddServiceEndpoint(typeof(ISomeContractDuplex), binding, address);
            svc.Open();

            ReceiverReadyMessage();

            InstanceContext instanceContext = new InstanceContext(new CallbackType());

            using (MyDuplexProxy proxy = new MyDuplexProxy(instanceContext, binding, new EndpointAddress(address))) {
                while (Console.ReadLine() != "QUIT") {
                    proxy.SomeOperation("Hi there");
                }
            }

            //svc.Close();
        }

        PrintHelper.Print("==== END Duplex NetTcpBinding Demo ====");
        PrintHelper.PrintNewLine();
    }

    private static void WsHttpBindingRequestReplyDemo() {
        PrintHelper.Print("==== BEGIN Request / Reply WsHttpBinding Demo ====");
  
        Uri address = new Uri("http://localhost:4000/ISomeContractRequestReply");
        DelegatorBinding binding = new DelegatorBinding(BindingMode.WSHttp);

        using (ServiceHost svc = new ServiceHost(typeof(RequestReplyReceiver))) {
            svc.AddServiceEndpoint(typeof(ISomeContractRequestReply), binding, address);
            svc.Open();

            ReceiverReadyMessage();

            using (MyRequestReplyProxy proxy = new MyRequestReplyProxy(binding, new EndpointAddress(address))) {
                while (Console.ReadLine() != "QUIT") {
                    proxy.SomeOperation("Hi there");
                }
            }
        }

        PrintHelper.Print("==== END Request / Reply WsHttpBinding Demo ====");
        PrintHelper.PrintNewLine();
    }

    private static void WsHttpBindingOneWayDemo() {
        PrintHelper.Print("==== BEGIN One Way WsHttpBinding Demo ====");
       
        Uri address = new Uri("http://localhost:4000/ISomeContractOneWay");
        DelegatorBinding binding = new DelegatorBinding(BindingMode.WSHttp);

        using (ServiceHost svc = new ServiceHost(typeof(OneWayReceiver))) {
            svc.AddServiceEndpoint(typeof(ISomeContractOneWay), binding, address);
            svc.Open();

            ReceiverReadyMessage();

            using (MyOneWayProxy proxy = new MyOneWayProxy(binding, new EndpointAddress(address))) {
                while (Console.ReadLine() != "QUIT") {
                    proxy.SomeOperation("Hi there");
                }
            }
        }

        PrintHelper.Print("==== END One Way WsHttpBinding Demo ====");
        PrintHelper.PrintNewLine();
    }

    private static void BasicHttpBindingRequestReplyDemo() {
        PrintHelper.Print("==== BEGIN Request / Reply BasicHttpBinding Demo ====");
    
        Uri address = new Uri("http://localhost:4000/ISomeContractRequestReply");
        DelegatorBinding binding = new DelegatorBinding(BindingMode.BasicHttp);

        using (ServiceHost svc = new ServiceHost(typeof(RequestReplyReceiver))) {
            svc.AddServiceEndpoint(typeof(ISomeContractRequestReply), binding, address);
            svc.Open();

            ReceiverReadyMessage();

            using (MyRequestReplyProxy proxy = new MyRequestReplyProxy(binding, new EndpointAddress(address))) {
                while (Console.ReadLine() != "QUIT") {
                    proxy.SomeOperation("Hi there");
                }
            }
        }

        PrintHelper.Print("==== END Request / Reply BasicHttpBinding Demo ====");
        PrintHelper.PrintNewLine();
    }

    private static void BasicHttpBindingOneWayDemo() {
        PrintHelper.Print("==== BEGIN One Way BasicHttpBinding Demo ====");
    
        Uri address = new Uri("http://localhost:4000/ISomeContractOneWay");
        DelegatorBinding binding = new DelegatorBinding(BindingMode.BasicHttp);
        
        using (ServiceHost svc = new ServiceHost(typeof(OneWayReceiver))) {
            svc.AddServiceEndpoint(typeof(ISomeContractOneWay), binding, address);

            svc.Open();

            ReceiverReadyMessage();

            using (MyOneWayProxy proxy = new MyOneWayProxy(binding, new EndpointAddress(address))) {
                while (Console.ReadLine() != "QUIT") {
                    proxy.SomeOperation("Hi there");
                }
            }
        }

        PrintHelper.Print("==== END One Way BasicHttpBinding Demo ====");
        PrintHelper.PrintNewLine();
    }

    private static void NetTcpBindingOneWayDemo() {
        PrintHelper.Print("==== BEGIN One Way NetTcpBinding Demo ====");
        
        Uri address = new Uri("net.tcp://localhost:4000/ISomeContractOneWay");
        DelegatorBinding binding = new DelegatorBinding(BindingMode.Tcp);

        using (MySericeHost svc = new MySericeHost(typeof(OneWayReceiver))) {
            svc.AddServiceEndpoint(typeof(ISomeContractOneWay), binding, address);
            svc.Open();

            ReceiverReadyMessage();

            using (MyOneWayProxy proxy = new MyOneWayProxy(binding, new EndpointAddress(address)))
            {
                while (Console.ReadLine() != "QUIT")
                {
                    proxy.SomeOperation("Hi there");
                }
            }
        }

        PrintHelper.Print("==== END One Way NetTcpBinding Demo ====");
        PrintHelper.PrintNewLine();
    }

    private class MySericeHost : ServiceHost {

        internal MySericeHost(Type type)
            : base(type) {

        }

        protected override void InitializeRuntime() {
            base.InitializeRuntime();
            
            Console.WriteLine("in initializeRuntime");
            ChannelDispatcher dispatcher = this.ChannelDispatchers[0] as ChannelDispatcher;
            dispatcher.ErrorHandlers.Add(new MyErrorHandler());
        }

    }

    private class MyErrorHandler : IErrorHandler {
           
        public bool  HandleError(Exception error)
        {
            Console.WriteLine("in handle error");
            Environment.FailFast(error.Message);
            return true;
        }

        public void  ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {

        }

}

    private static void NetTcpBindingRequestReplyDemo() {

        PrintHelper.Print("==== BEGIN Request / Reply NetTcpBinding Demo ====");
        
        Uri address = new Uri("net.tcp://localhost:4000/ISomeContractRequestReply");
        DelegatorBinding binding = new DelegatorBinding(BindingMode.Tcp);

        using (ServiceHost svc = new ServiceHost(typeof(RequestReplyReceiver))) {
            svc.AddServiceEndpoint(typeof(ISomeContractRequestReply), binding, address);
            svc.Open();

            ReceiverReadyMessage();

            using (MyRequestReplyProxy proxy = new MyRequestReplyProxy(binding, new EndpointAddress(address))) {
                while (Console.ReadLine() != "QUIT") {
                    proxy.SomeOperation("Hi there");
                }
            }
        }

        PrintHelper.Print("==== END Request / Reply NetTcpBinding Demo ====");
        PrintHelper.PrintNewLine();
    }

    private static void ReceiverReadyMessage() {
        String message = String.Format("{0}THE RECEIVER IS READY{0}PRESS ENTER TO SEND A MESSAGE, QUIT TO GO TO NEXT DEMO{0}", Environment.NewLine);
        PrintHelper.PrintNoThread(message);
    }

}

#region RECEIVER TYPE DEFS

internal sealed class RequestReplyReceiver : ISomeContractRequestReply {
    public RequestReplyReceiver() {
        PrintHelper.Print("RECEIVER OBJECT: RequestReplyReceiver", "ctor");
    }

    // ISomeContract implementation
    public String SomeOperation(String input) {
        PrintHelper.Print("RECEIVER OBJECT: RequestReplyReceiver", String.Format("SomeOperation: {0}", input));
        
        Char[] chars = input.ToCharArray();
        Array.Reverse(chars);
        return new String(chars);
    }
}

internal sealed class OneWayReceiver : ISomeContractOneWay {
    public OneWayReceiver() {
        PrintHelper.Print("RECEIVER OBJECT: OneWayReceiver", "ctor");
    }

    // ISomeContract implementation
    public void SomeOperation(String input) {
        PrintHelper.Print("RECEIVER OBJECT: OneWayReceiver", String.Format("SomeOperation: {0}", input));
    }
}

internal sealed class DuplexReceiver : ISomeContractDuplex {
    
    public DuplexReceiver() {
        PrintHelper.Print("RECEIVER OBJECT: DuplexReceiver", "ctor");
    }

    // ISomeContract implementation
    public void SomeOperation(String input) {
        PrintHelper.Print("RECEIVER OBJECT: DuplexReceiver", String.Format("SomeOperation: {0}", input));

        Char[] chars = input.ToCharArray();
        Array.Reverse(chars);
        String retval = new String(chars);

        // use the callback channel to return a reversed String
        ICallbackContract callback = OperationContext.Current.GetCallbackChannel<ICallbackContract>();
        callback.SomeCallbackOperation(retval);
    }
}

internal sealed class OneWaySessionfulReceiver : ISomeContractOneWaySession {

    internal OneWaySessionfulReceiver() {
        PrintHelper.Print("RECEIVER OBJECT: OneWaySessionfulReceiver", "ctor");
    }

    public void SomeOperation(String input) {
        PrintHelper.Print("RECEIVER OBJECT: OneWaySessionfulReceiver", String.Format("SomeOperation: {0}", input));
    }

    public void SomeOtherOperation(String input) {
        PrintHelper.Print("RECEIVER OBJECT: OneWaySessionfulReceiver", String.Format("SomeOtherOperation: {0}", input));
    }
}

[ServiceBehavior(InstanceContextMode=InstanceContextMode.PerSession)]
internal sealed class OneWaySessionfulTransactionalReceiver : ISomeContractOneWaySession {

    internal OneWaySessionfulTransactionalReceiver() {
        PrintHelper.Print("RECEIVER OBJECT: OneWaySessionfulTransactionalReceiver", "ctor");
    }

    [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete=false)]
    public void SomeOperation(String input) {
        PrintHelper.Print("RECEIVER OBJECT: OneWaySessionfulTransactionalReceiver", String.Format("SomeOperation: {0}", input));
    }
    [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete=true)]
    public void SomeOtherOperation(String input) {
        PrintHelper.Print("RECEIVER OBJECT: OneWaySessionfulTransactionalReceiver", String.Format("SomeOtherOperation: {0}", input));
    }
}

#endregion

#region CONTRACTS

// Basic One-Way Contract
[ServiceContract]
internal interface ISomeContractOneWay {
    [OperationContract(IsOneWay=true)]
    void SomeOperation(String input);
}

// Basic Request / Reply Contract
[ServiceContract]
internal interface ISomeContractRequestReply
{
    [OperationContract]
    String SomeOperation(String input);
}

// Duplex Contract
[ServiceContract(CallbackContract=typeof(ICallbackContract))]
internal interface ISomeContractDuplex {
    [OperationContract(IsOneWay=true)]
    void SomeOperation(String input);
}

internal interface ICallbackContract {
    [OperationContract(IsOneWay = true)]
    void SomeCallbackOperation(String input);
}

// OneWay Sessionful Contract
[ServiceContract(SessionMode=SessionMode.Required)]
interface ISomeContractOneWaySession {
    [OperationContract(IsInitiating=true, IsOneWay=true)]
    void SomeOperation(String input);
    [OperationContract(IsTerminating = true, IsOneWay=true)]
    void SomeOtherOperation(String input);
}

#endregion

#region CLIENTBASE TYPES

internal sealed class MyRequestReplyProxy : ClientBase<ISomeContractRequestReply>, ISomeContractRequestReply
{
    internal MyRequestReplyProxy(Binding binding, EndpointAddress address) : base(binding, address)
    {     
    }

    protected override ISomeContractRequestReply CreateChannel()
    {
        return base.CreateChannel();
    }

    public String SomeOperation(String input)
    {
        PrintHelper.Print("SENDER: MyRequestReplyProxy", "SomeOperation");
        String retVal = base.Channel.SomeOperation(input);
        PrintHelper.Print("SENDER: MyRequestReplyProxy", String.Format("SomeOperation returned \"{0}\"", retVal));
        return retVal;
    }
}

internal sealed class MyOneWayProxy : ClientBase<ISomeContractOneWay>, ISomeContractOneWay {
    internal MyOneWayProxy(Binding binding, EndpointAddress address)
        : base(binding, address) {
    }

    protected override ISomeContractOneWay CreateChannel() {
        return base.CreateChannel();
    }

    public void SomeOperation(String input) {
        PrintHelper.Print("SENDER: ISomeContractOneWay", "SomeOperation");
        base.Channel.SomeOperation(input);
    }
}

internal sealed class MyOneWaySessionfulProxy : ClientBase<ISomeContractOneWaySession>, ISomeContractOneWaySession {
    internal MyOneWaySessionfulProxy(Binding binding, EndpointAddress address)
        : base(binding, address) {
    }

    protected override ISomeContractOneWaySession CreateChannel() {
        return base.CreateChannel();
    }

    public void SomeOperation(String input) {
        PrintHelper.Print("SENDER: ISomeContractOneWaySession", "SomeOperation");
        base.Channel.SomeOperation(input);
    }

    public void SomeOtherOperation(String input) {
        PrintHelper.Print("SENDER: ISomeContractOneWaySession", "SomeOtherOperation");
        base.Channel.SomeOtherOperation(input);
    }
}

internal sealed class MyDuplexProxy : ClientBase<ISomeContractDuplex>, ISomeContractDuplex {

    internal MyDuplexProxy(InstanceContext instanceContext, Binding binding, EndpointAddress address)
        : base(instanceContext, binding, address) {
    }

    protected override ISomeContractDuplex CreateChannel() {
        return base.CreateChannel();
    }

    public void SomeOperation(String input) {
        PrintHelper.Print("SENDER: ISomeContractDuplex", "SomeOperation");
        base.Channel.SomeOperation(input);
    }
}

internal sealed class CallbackType : ICallbackContract {

    internal CallbackType() {
        PrintHelper.Print("CallbackType", "ctor");
    }

    public void SomeCallbackOperation(String input) {
        PrintHelper.Print("CallbackType", String.Format("SomeCalbackOperation: {0}", input));
    }
}

#endregion


