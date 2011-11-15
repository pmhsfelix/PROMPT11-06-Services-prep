using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;

// type that can be used directly, but must be used with the IDuplexSessionChannel shape
// the other channel examples show how to build a type hierarchy for multiple channel shapes
internal sealed class MyDelegatorDuplexChannel : ChannelBase, IDuplexSessionChannel {

    // reference the next channel in the channel stack
    private IDuplexSessionChannel innerChannel;

    private String _consolePrefix;

    internal MyDelegatorDuplexChannel(ChannelManagerBase channelManagerBase,
        IDuplexSessionChannel innerChannel, String consolePrefix) : base(channelManagerBase) {
        this.innerChannel = innerChannel;
        _consolePrefix = consolePrefix;
    }

    public IAsyncResult BeginReceive(TimeSpan timeout, AsyncCallback callback, object state) {
        PrintHelper.Print(_consolePrefix, "BeginReceive");
        return innerChannel.BeginReceive(timeout, callback, state);
    }

    public IAsyncResult BeginReceive(AsyncCallback callback, object state) {
        PrintHelper.Print(_consolePrefix, "BeginReceive");
        return innerChannel.BeginReceive(callback, state);
    }

    public IAsyncResult BeginTryReceive(TimeSpan timeout, AsyncCallback callback, object state) {
        PrintHelper.Print(_consolePrefix, "BeginTryReceive");
        return innerChannel.BeginTryReceive(timeout, callback, state);
    }

    public IAsyncResult BeginWaitForMessage(TimeSpan timeout, AsyncCallback callback, object state) {
        PrintHelper.Print(_consolePrefix, "BeginWaitForMessage");
        return innerChannel.BeginWaitForMessage(timeout, callback, state);
    }

    public Message EndReceive(IAsyncResult result) {
        PrintHelper.Print(_consolePrefix, "EndReceive");
        return innerChannel.EndReceive(result);
    }

    public bool EndTryReceive(IAsyncResult result, out Message message) {
        PrintHelper.Print(_consolePrefix, "EndTryReceive");
        return innerChannel.EndTryReceive(result, out message);
    }

    public bool EndWaitForMessage(IAsyncResult result) {
        PrintHelper.Print(_consolePrefix, "EndWaitForMessage");
        return innerChannel.EndWaitForMessage(result);
    }

    public EndpointAddress LocalAddress {
        get {
            PrintHelper.Print(_consolePrefix, "LocalAddress");
            return innerChannel.LocalAddress; }
    }

    public Message Receive(TimeSpan timeout) {
        PrintHelper.Print(_consolePrefix, "Receive");
        return innerChannel.Receive(timeout);
    }

    public Message Receive() {
        PrintHelper.Print(_consolePrefix, "Receive, no TimeSpan arg");
        return this.Receive(this.DefaultReceiveTimeout);
    }

    public bool TryReceive(TimeSpan timeout, out Message message) {
        PrintHelper.Print(_consolePrefix, "TryReceive");
        return innerChannel.TryReceive(timeout, out message);
    }

    public bool WaitForMessage(TimeSpan timeout) {
        PrintHelper.Print(_consolePrefix, "WaitForMessage");
        return innerChannel.WaitForMessage(timeout);
    }

    public IAsyncResult BeginSend(Message message, TimeSpan timeout, AsyncCallback callback, object state) {
        PrintHelper.Print(_consolePrefix, "BeginSend");
        return innerChannel.BeginSend(message, timeout, callback, state);
    }

    public IAsyncResult BeginSend(Message message, AsyncCallback callback, object state) {
        PrintHelper.Print(_consolePrefix, "BeginSend, no TimeSpan arg");
        return this.BeginSend(message, this.DefaultSendTimeout, callback, state);
    }

    public void EndSend(IAsyncResult result) {
        PrintHelper.Print(_consolePrefix, "EndSend");
        innerChannel.EndSend(result);
    }

    public EndpointAddress RemoteAddress {
        get {
            PrintHelper.Print(_consolePrefix, "RemoteAddress");
            return innerChannel.RemoteAddress;  
        }
    }

    public void Send(Message message, TimeSpan timeout) {
        PrintHelper.Print(_consolePrefix, "Send");
        innerChannel.Send(message, timeout);
    }

    public void Send(Message message) {
        PrintHelper.Print(_consolePrefix, "Send, no TimeSpan arg");
        this.Send(message, this.DefaultSendTimeout);
    }

    public Uri Via {
        get {
            PrintHelper.Print(_consolePrefix, "Via");
            return innerChannel.Via; 
        }
    }

    public IDuplexSession Session {
        get {
            PrintHelper.Print(_consolePrefix, "Session");
            return innerChannel.Session; 
        }
    }

    protected override void OnAbort() {
        PrintHelper.Print(_consolePrefix, "OnAbort");
        innerChannel.Abort();
    }

    protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state) {
        PrintHelper.Print(_consolePrefix, "OnBeginClose");
        return innerChannel.BeginClose(timeout, callback, state);
    }

    protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state) {
        PrintHelper.Print(_consolePrefix, "OnBeginOpen");
        return innerChannel.BeginOpen(timeout, callback, state);
    }

    protected override void OnClose(TimeSpan timeout) {
        PrintHelper.Print(_consolePrefix, "OnClose");
        innerChannel.Close(timeout);
    }

    protected override void OnEndClose(IAsyncResult result) {
        PrintHelper.Print(_consolePrefix, "OnEndClose");
        innerChannel.EndClose(result);
    }

    protected override void OnEndOpen(IAsyncResult result) {
        PrintHelper.Print(_consolePrefix, "OnEndOpen");
        innerChannel.EndOpen(result);
    }

    protected override void OnOpen(TimeSpan timeout) {
        PrintHelper.Print(_consolePrefix, "OnOpen");
        innerChannel.Open(timeout);
    }

    public override T GetProperty<T>() {
        return innerChannel.GetProperty<T>();
    }

}

