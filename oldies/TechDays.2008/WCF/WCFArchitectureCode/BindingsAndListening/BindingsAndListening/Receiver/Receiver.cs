using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Collections.Generic;

internal sealed class ReceiverApp {

  static void Main(string[] args){

    Binding binding = new CustomBinding(new HttpTransportBindingElement());

    Uri address = new Uri("http://localhost:8000/SomeService");
    // build a channel listener with Channel Shape
    IChannelListener<IReplyChannel> listener = binding.BuildChannelListener<IReplyChannel>(address);
    
    // start listening
    listener.Open();

    // get the channel from the ChannelListener
    IReplyChannel channel = listener.AcceptChannel();

    // receive messages on this channel
    channel.Open();

    Console.WriteLine("the receiver is ready");

    // Block until message is received
    RequestContext context = channel.ReceiveRequest();
    Message msg = context.RequestMessage;

    Console.WriteLine(msg.ToString());

    Message responseMessage = Message.CreateMessage(MessageVersion.Soap12WSAddressing10, "urn:SomeActionResponse", "hello back");

    context.Reply(responseMessage);

    Console.ReadLine();
  }
}



