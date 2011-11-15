using System;
using System.Xml;
using System.ServiceModel;
using System.ServiceModel.Channels;

// define the contract for the service
[ServiceContract(Namespace = "http://contoso.com/Eratosthenes")]
public interface IEratosthenes
{
    [OperationContract()]
    Int32 Sieve(Int32 number);
}

