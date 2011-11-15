using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Reflection;
using System.Collections.Generic;
using System.ServiceModel.MsmqIntegration;

sealed class BindingElementsShow
{
    static void Main()
    {
        List<Binding> bindings = new List<Binding>();
        bindings.Add(new BasicHttpBinding());
        bindings.Add(new NetNamedPipeBinding());
        bindings.Add(new NetTcpBinding());
        bindings.Add(new NetTcpBinding(SecurityMode.Message, true));
        bindings.Add(new WSDualHttpBinding());
        bindings.Add(new WSHttpBinding());
        bindings.Add(new NetMsmqBinding());
        bindings.Add(new NetMsmqBinding(NetMsmqSecurityMode.Message));
        bindings.Add(new MsmqIntegrationBinding());
        bindings.Add(new WSFederationHttpBinding());
        bindings.Add(new WebHttpBinding());

        ShowBindingElements(bindings);
        
    }

    private static void ShowBindingElements(List<Binding> bindings)
    {
        foreach (Binding binding in bindings)
        {
            Console.WriteLine("Showing Binding Elements for {0}", binding.GetType().Name);
            foreach (BindingElement element in binding.CreateBindingElements())
            {
                Console.WriteLine("\t{0}", element.GetType().Name);
            }
            Console.WriteLine();
        }
    }
}
