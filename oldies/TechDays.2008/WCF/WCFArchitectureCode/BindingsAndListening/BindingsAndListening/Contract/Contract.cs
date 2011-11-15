using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

[ServiceContract(Name = "SomeContract",
                 Namespace = "http://contoso.com/SomeContract")]
public interface ISomeContract
{
  [OperationContract(Name = "SomeContract",
                     Action = "urn:SomeAction",
                     IsOneWay = true)]
  void SomeOperation(Message message);
}

