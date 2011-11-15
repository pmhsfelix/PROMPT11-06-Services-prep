using System;
using System.ServiceModel;
using System.ServiceModel.Channels;


sealed class Sender
{
    static void Main()
    {
        Console.WriteLine("Press ENTER when service is ready");
        Console.ReadLine();

        WSHttpBinding binding = new WSHttpBinding();

        EndpointAddress address = new EndpointAddress("http://localhost:8000/Eratosthenes");

        ChannelFactory<IEratosthenes> factory = new ChannelFactory<IEratosthenes>(binding, address);

        MyValidatorAttribute myValidator = new MyValidatorAttribute();
        myValidator.Min = 1;
        myValidator.Max = 200;
        myValidator.Index = 0;

        factory.Endpoint.Contract.Operations.Find("Sieve").Behaviors.Add(myValidator);

        IEratosthenes proxy = factory.CreateChannel();

        Int32 num = 100000000;

        Console.WriteLine("Sieve of {0} is {1}", num, proxy.Sieve(num));
        Console.WriteLine("\n==================================================\n");
        Console.WriteLine("Sieve of {0} is {1}", num, proxy.Sieve(num));

        ((IClientChannel)proxy).Dispose();

        Console.WriteLine("\nPress any key to exit");
        Console.Read();

    }
}

