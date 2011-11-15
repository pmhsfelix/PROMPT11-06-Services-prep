using System;
using System.Xml;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Collections;

sealed class ExtendingDispatcherReceiver
{
    static void Main()
    {
        // define the binding for the service
        WSHttpBinding binding = new WSHttpBinding();

        // define the address for the service
        Uri addressURI = new Uri("http://localhost:8000/Eratosthenes");

        // instantiate a Service host using the MyService type
        using (ServiceHost svc = new ServiceHost(typeof(MyService)))
        {
            // add an endpoint to the service with the address, contract, and binding
            svc.AddServiceEndpoint(typeof(IEratosthenes), binding, addressURI);

            ServiceEndpointCollection endpoints = svc.Description.Endpoints;
            foreach (ServiceEndpoint endpoint in endpoints)
            {
                Console.WriteLine(endpoint.Address.ToString());
            }

            foreach (IServiceBehavior behavior in svc.Description.Behaviors)
            {
                Console.WriteLine(behavior.ToString());
            }

            // open the service to start listening
            svc.Open();
            Console.WriteLine("The service is ready");
            Console.ReadLine();
        }
    }
}

sealed class MyService : IEratosthenes
{
    [MyCacher]
    [MyValidator(Index=1, Max=100000, Min=0)]
    public Int32 Sieve(Int32 max)
    {
        BitArray bits = new BitArray(max + 1, true);

        Int32 limit = 2;
        while (limit * limit < max)
            limit++;

        for (Int32 i = 2; i <= limit; i++)
        {
            if (bits[i])
            {
                for (Int32 k = i + i; k <= max; k += i)
                    bits[k] = false;
            }
        }

        Int32 count = 0;
        for (Int32 i = 2; i <= max; i++)
        {
            if (bits[i])
                count++;
        }

        return count;
    }

}
