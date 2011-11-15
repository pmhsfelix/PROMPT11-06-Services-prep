using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Reflection;
using System.Web.Services.Description;

namespace RequireAttribute
{
    //-- The service's contract
    [ServiceContract]
    public interface IMyService
    {
        [OperationContract]
        [RequireNonNull("s")] //<-- A custom behavior as an attribute to the contract
        string MyOperation(int i, string s);
    }

    // -- The service's implementation
    class MyServiceImpl : IMyService
    {
        #region IMyService Members

        public string MyOperation(int i, string s)
        {
            //return s.ToUpper();
            return "HELLO";
        }

        #endregion
    }

    [AttributeUsage(AttributeTargets.Method,AllowMultiple=false)]
    public class RequireNonNullAttribute : Attribute, IOperationBehavior, IParameterInspector, IWsdlExportExtension
    {
        string name;
        int index;
        OperationDescription od;
        public RequireNonNullAttribute(string n)
        {
            name = n;
        }

        #region IOperationBehavior Members

        public void AddBindingParameters(OperationDescription operationDescription, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            // Nothing to do here: no need to add parameters to the binding elements
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.ClientOperation clientOperation)
        {
            // Nothing to do here: this behavior does not apply on the client side
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.DispatchOperation dispatchOperation)
        {
            dispatchOperation.ParameterInspectors.Add(this);
        }

        public void Validate(OperationDescription operationDescription)
        {
            // Get ParameterInfo for the required parameter
            MethodInfo mi = operationDescription.SyncMethod;
            var pi = mi.GetParameters().Where(x => x.Name == name).FirstOrDefault();
            if (pi == null)
            {
                throw new Exception(string.Format("The parameter {0} does not exists", name));
            }
            // Save its index and also the OperationDescription
            index = pi.Position;
            od = operationDescription;
        }

        #endregion

        #region IParameterInspector Members

        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
            
        }

        public object BeforeCall(string operationName, object[] inputs)
        {
            //check is parameter is null and fault if it is
            if (inputs[index] == null)
            {
                throw new FaultException(string.Format("Parameter {0} cannot be null",name));
            }
            return null;            
        }

        #endregion

        #region IWsdlExportExtension Members

        public void ExportContract(WsdlExporter exporter, WsdlContractConversionContext context)
        {
            // Insert a <wsdl:documentation> on the generated WSDL
            Operation oper = context.GetOperation(od);
            OperationMessage message = oper.Messages[0];
            message.Documentation = string.Format("Parameter {0} cannot be null", name);

        }

        public void ExportEndpoint(WsdlExporter exporter, WsdlEndpointConversionContext context)
        {
            // Nothing to do here
        }

        #endregion
    }

    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost sh = new ServiceHost(typeof(MyServiceImpl));
            sh.AddServiceEndpoint(typeof(IMyService), new BasicHttpBinding(), "http://coruja:8080/ra");
            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetUrl = new Uri("http://coruja:8080/ra/metadata");
            smb.HttpGetEnabled = true;
            sh.Description.Behaviors.Add(smb);
            sh.Open();
            Console.WriteLine("service is opened");
            Console.ReadKey();

            ChannelFactory<IMyService> cf = new ChannelFactory<IMyService>(new BasicHttpBinding(), new EndpointAddress("http://coruja:8080/ra"));
            IMyService ch = cf.CreateChannel();
            Console.WriteLine(ch.MyOperation(0,null));
            Console.ReadKey();

        }
    }
}
