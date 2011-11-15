using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

internal sealed class Sender
{
  static void Main(String[] args)
  {
    Binding binding = new CustomBinding(new HttpTransportBindingElement());

    IChannelFactory<IRequestChannel> factory = binding.BuildChannelFactory<IRequestChannel>();

    factory.Open();

    EndpointAddress address = new EndpointAddress("http://localhost:8000/SomeService");

    IRequestChannel channel = factory.CreateChannel(address);

    channel.Open();

    Message requestMsg = Message.CreateMessage(MessageVersion.Soap12WSAddressing10, "urn:SomeAction", "Hello WCF");

    Message responseMsg = channel.Request(requestMsg, new TimeSpan(0, 0, 5));

    Console.WriteLine(responseMsg.ToString());

    Console.ReadLine();

  }
}

