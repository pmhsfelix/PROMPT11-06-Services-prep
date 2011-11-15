using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel;

internal class DelegatorInputChannel<TShape> : DelegatorChannelBase<TShape>, IInputChannel where TShape:class, IInputChannel {

    private String _source; // store the String to output

    internal DelegatorInputChannel(ChannelManagerBase channelManagerBase, TShape innerChannel, String source)
        : base(channelManagerBase, innerChannel, source) {
        // assign the name and generic parameter to the String
        _source = String.Format("{0} CHANNEL: DelegatorInputChannel<{1}>", source, typeof(TShape).Name);
        // output that the method was called
        PrintHelper.Print(_source, "ctor");
    }

    #region IInputChannel Members

    public IAsyncResult BeginReceive(TimeSpan timeout, AsyncCallback callback, object state) {
        PrintHelper.Print(_source, "BeginReceive");
        return this.InnerChannel.BeginReceive(timeout, callback, state);
    }

    public IAsyncResult BeginReceive(AsyncCallback callback, object state) {
        PrintHelper.Print(_source, "BeginReceive, no TimeSpan arg");
        return this.BeginReceive(this.DefaultReceiveTimeout, callback, state);
    }

    public IAsyncResult BeginTryReceive(TimeSpan timeout, AsyncCallback callback, object state) {
        PrintHelper.Print(_source, "BeginTryReceive");

        //Console.WriteLine("poo code");
        //throw new Exception("whoops");

        return this.InnerChannel.BeginTryReceive(timeout, callback, state);
    }

    public IAsyncResult BeginWaitForMessage(TimeSpan timeout, AsyncCallback callback, object state) {
        PrintHelper.Print(_source, "BeginWaitForMessage");
        return this.InnerChannel.BeginWaitForMessage(timeout, callback, state);
    }

    public Message EndReceive(IAsyncResult result) {
        PrintHelper.Print(_source, "EndReceive");
        return this.InnerChannel.EndReceive(result);
    }

    public bool EndTryReceive(IAsyncResult result, out Message message) {
        PrintHelper.Print(_source, "EndTryReceive");
        return this.InnerChannel.EndTryReceive(result, out message);
    }

    public bool EndWaitForMessage(IAsyncResult result) {
        PrintHelper.Print(_source, "EndWaitForMessage");
        return this.InnerChannel.EndWaitForMessage(result);
    }

    public EndpointAddress LocalAddress {
        get {
            PrintHelper.Print(_source, "LocalAddress");
            return this.InnerChannel.LocalAddress; }
    }

    public Message Receive(TimeSpan timeout) {
        PrintHelper.Print(_source, "Receive");
        return this.InnerChannel.Receive(timeout);
    }

    public Message Receive() {
        PrintHelper.Print(_source, "Receive, no TimeSpan arg");
        return this.Receive(this.DefaultReceiveTimeout);
    }

    public bool TryReceive(TimeSpan timeout, out Message message) {
        PrintHelper.Print(_source, "TryReceive (BLOCKING)");
        bool messageReceived = this.InnerChannel.TryReceive(timeout, out message);
        
        if (messageReceived == false) {
            PrintHelper.Print(_source, "TryReceive Returned no Message");
        }

        if (messageReceived == true && message == null){
            PrintHelper.Print(_source, "TryReceive Returned true, but no message");
        }

        if (messageReceived == true && message != null) {
            PrintHelper.Print(_source, "TryReceive Returned true and a message");
        }

        return messageReceived;
    }

    public bool WaitForMessage(TimeSpan timeout) {
        PrintHelper.Print(_source, "WaitForMessage");
        return this.InnerChannel.WaitForMessage(timeout);
    }

    #endregion
}

internal sealed class DelegatorInputSessionChannel : DelegatorInputChannel<IInputSessionChannel>, IInputSessionChannel {

    private IInputSessionChannel _inputSessionChannel;
    private String _source; // store the string to output

    internal DelegatorInputSessionChannel(ChannelManagerBase channelManagerBase, IInputSessionChannel innerChannel, String source)
        : base(channelManagerBase, innerChannel, source) {
        
        _source = String.Format("{0} CHANNEL: DelegatorInputSessionChannel", source);
        PrintHelper.Print(_source, "ctor");
        this._inputSessionChannel = innerChannel;
    }

    #region ISessionChannel<IInputSession> Members

    public IInputSession Session {
        get {
            PrintHelper.Print(_source, "Session");
            return this._inputSessionChannel.Session; }
    }

    #endregion
}

internal class DelegatorOutputChannel<TShape> : DelegatorChannelBase<TShape>, IOutputChannel where TShape: class, IOutputChannel {

    private String _source; // store the String to output

    internal DelegatorOutputChannel(ChannelManagerBase channelManagerBase, TShape innerChannel, String source)
        : base(channelManagerBase, innerChannel, source) {
        
        _source = String.Format("{0} CHANNEL: DelegatorOutputChannel<{1}>", source, typeof(TShape).Name);
        PrintHelper.Print(_source, "ctor");
    }

    #region IOutputChannel Members

    public IAsyncResult BeginSend(Message message, TimeSpan timeout, AsyncCallback callback, object state) {
        PrintHelper.Print(_source, "BeginSend");
        return this.InnerChannel.BeginSend(message, timeout, callback, state);
    }

    public IAsyncResult BeginSend(Message message, AsyncCallback callback, object state) {
        PrintHelper.Print(_source, "BeginSend, no TimeSpan arg");
        return this.BeginSend(message, this.DefaultSendTimeout, callback, state);
    }

    public void EndSend(IAsyncResult result) {
        PrintHelper.Print(_source, "EndSend");
        this.InnerChannel.EndSend(result);
    }

    public EndpointAddress RemoteAddress {
        get {
            PrintHelper.Print(_source, "RemoteAddress");
            return this.InnerChannel.RemoteAddress; }
    }

    public void Send(Message message, TimeSpan timeout) {
        PrintHelper.Print(_source, "Send");
        this.InnerChannel.Send(message, timeout);
    }

    public void Send(Message message) {
        PrintHelper.Print(_source, "Send");
        this.Send(message, this.DefaultSendTimeout);
    }

    public Uri Via {
        get {
            PrintHelper.Print(_source, "Via");
            return this.InnerChannel.Via; }
    }

    #endregion
}

internal sealed class DelegatorOutputSessionChannel : DelegatorOutputChannel<IOutputSessionChannel>, IOutputSessionChannel {

    private IOutputSessionChannel _innerSessionChannel;
    private String _source;

    internal DelegatorOutputSessionChannel(ChannelManagerBase channelManagerBase, IOutputSessionChannel innerChannel, String source)
        : base(channelManagerBase, innerChannel, source) {
        
        _source = String.Format("{0} CHANNEL: DelegatorOutputSessionChannel", source);
        PrintHelper.Print(_source, "ctor");
        this._innerSessionChannel = innerChannel;
    }

    #region ISessionChannel<IOutputSession> Members

    public IOutputSession Session {
        get {
            PrintHelper.Print(_source, "Session");
            return this._innerSessionChannel.Session; }
    }

    #endregion

}

internal class DelegatorDuplexChannel : DelegatorInputChannel<IDuplexChannel>, IDuplexChannel {

    private String _source; // store the String to output

    internal DelegatorDuplexChannel(ChannelManagerBase channelManagerBase, IDuplexChannel innerChannel, String source)
        : base(channelManagerBase, innerChannel, source) {
  
        _source = String.Format("{0} CHANNEL: DelegatorDuplexChannel", source);
        PrintHelper.Print(_source, "ctor");
    }

    #region IOutputChannel Members

    public IAsyncResult BeginSend(Message message, TimeSpan timeout, AsyncCallback callback, object state) {
        PrintHelper.Print(_source, "BeginSend");
        return this.InnerChannel.BeginSend(message, timeout, callback, state);
    }

    public IAsyncResult BeginSend(Message message, AsyncCallback callback, object state) {
        PrintHelper.Print(_source, "BeginSend, no TimeSpan arg");
        return this.BeginSend(message, this.DefaultSendTimeout, callback, state);
    }

    public void EndSend(IAsyncResult result) {
        PrintHelper.Print(_source, "EndSend");
        this.InnerChannel.EndSend(result);
    }

    public EndpointAddress RemoteAddress {
        get {
            PrintHelper.Print(_source, "RemoteAddress");
            return this.InnerChannel.RemoteAddress; }
    }

    public void Send(Message message, TimeSpan timeout) {
        PrintHelper.Print(_source, "Send");
        this.InnerChannel.Send(message, timeout);
    }

    public void Send(Message message) {
        PrintHelper.Print(_source, "Send, no TimeSpan arg");
        this.Send(message, this.DefaultSendTimeout);
    }

    public Uri Via {
        get {
            PrintHelper.Print(_source, "Via");
            return this.InnerChannel.Via; }
    }

    #endregion
}

internal sealed class DelegatorDuplexSessionChannel : DelegatorDuplexChannel, IDuplexSessionChannel {

    private IDuplexSessionChannel innerSessionChannel; // reference the next sessionful channel
    private String _source;  // store the String to output

    internal DelegatorDuplexSessionChannel(ChannelManagerBase channelManagerBase, IDuplexSessionChannel innerChannel, String source)
        : base(channelManagerBase, innerChannel, source) {
        _source = String.Format("{0} CHANNEL: DelegatorDuplexSessionChannel", source);
        //
        PrintHelper.Print(_source, "ctor");
        this.innerSessionChannel = innerChannel;
    }

    public IDuplexSession Session {
        get {
            PrintHelper.Print(_source, "Session");
            return this.innerSessionChannel.Session; }
    }

}

internal class DelegatorRequestChannel : DelegatorChannelBase<IRequestChannel>, IRequestChannel {

    String _source;

    internal DelegatorRequestChannel(ChannelManagerBase channelManagerBase, IRequestChannel innerChannel, String source)
        : base(channelManagerBase, innerChannel, source) {
        _source = String.Format("{0} CHANNEL: DelegatorRequestChannel", source);
        PrintHelper.Print(_source, "ctor");
    }

    #region IRequestChannel Members

    public IAsyncResult BeginRequest(Message message, TimeSpan timeout, AsyncCallback callback, object state) {
        PrintHelper.Print(_source, "BeginRequest");
        return this.InnerChannel.BeginRequest(message, timeout, callback, state);
    }

    public IAsyncResult BeginRequest(Message message, AsyncCallback callback, object state) {
        PrintHelper.Print(_source, "BeginRequest, no TimeSpan arg");
        return this.BeginRequest(message, this.DefaultSendTimeout, callback, state);
    }

    public Message EndRequest(IAsyncResult result) {
        PrintHelper.Print(_source, "EndRequest");
        return this.InnerChannel.EndRequest(result);
    }

    public EndpointAddress RemoteAddress {
        get {
            PrintHelper.Print(_source, "RemoteAddress");
            return this.InnerChannel.RemoteAddress; }
    }

    public Message Request(Message message, TimeSpan timeout) {
        PrintHelper.Print(_source, "Request (BLOCKING)");
        Message retMessage = this.InnerChannel.Request(message, timeout);
        if (retMessage == null) {
            PrintHelper.Print(_source, "Request Returned no Message");
        }
        else {
            PrintHelper.Print(_source, "Request Returned a message");
        }
        return retMessage;
    }

    public Message Request(Message message) {
        PrintHelper.Print(_source, "Request (BLOCKING), no TimeSpan arg");
        return this.Request(message, this.DefaultSendTimeout);
    }

    public Uri Via {
        get {
            PrintHelper.Print(_source, "Via");
            return this.InnerChannel.Via; }
    }

    #endregion
}

internal sealed class DelegatorRequestSessionChannel : DelegatorRequestChannel, IRequestSessionChannel {

    private IRequestSessionChannel _innerSessionChannel;
    private String _source;

    internal DelegatorRequestSessionChannel(ChannelManagerBase channelManagerBase, IRequestSessionChannel innerChannel, String source)
        : base(channelManagerBase, innerChannel, source) {
 
        _source = String.Format("{0} CHANNEL: DelegatorRequestSessionChannel", source);
        PrintHelper.Print(_source, "ctor");
        this._innerSessionChannel = innerChannel;
    }

    #region ISessionChannel<IOutputSession> Members

    public IOutputSession Session {
        get {
            PrintHelper.Print(_source, "Session");
            return this._innerSessionChannel.Session; }
    }

    #endregion

}

internal class DelegatorReplyChannel : DelegatorChannelBase<IReplyChannel>, IReplyChannel {

    private String _source;

    internal DelegatorReplyChannel(ChannelManagerBase channelManagerBase, IReplyChannel innerChannel, String source)
        : base(channelManagerBase, innerChannel, source) {
       
        _source = String.Format("{0} CHANNEL: DelegatorReplyChannel", source);
        PrintHelper.Print(_source, "ctor");
    }

    #region IReplyChannel Members

    public IAsyncResult BeginReceiveRequest(TimeSpan timeout, AsyncCallback callback, object state) {
        PrintHelper.Print(_source, "BeginReceiveRequest");
        return this.InnerChannel.BeginReceiveRequest(timeout, callback, state);
    }

    public IAsyncResult BeginReceiveRequest(AsyncCallback callback, object state) {
        PrintHelper.Print(_source, "BeginReceiveRequest, no TimeSpan arg");
        return this.BeginReceiveRequest(this.DefaultReceiveTimeout, callback, state);
    }

    public IAsyncResult BeginTryReceiveRequest(TimeSpan timeout, AsyncCallback callback, object state) {
        PrintHelper.Print(_source, "BeginTryReceiveRequest");
        return this.InnerChannel.BeginTryReceiveRequest(timeout, callback, state);
    }

    public IAsyncResult BeginWaitForRequest(TimeSpan timeout, AsyncCallback callback, object state) {
        PrintHelper.Print(_source, "BeginWaitForRequest");
        return this.InnerChannel.BeginWaitForRequest(timeout, callback, state);
    }

    public RequestContext EndReceiveRequest(IAsyncResult result) {
        PrintHelper.Print(_source, "EndReceiveRequest");
        return this.InnerChannel.EndReceiveRequest(result);
    }

    public bool EndTryReceiveRequest(IAsyncResult result, out RequestContext context) {
        PrintHelper.Print(_source, "EndTryReceiveRequest");
        return this.InnerChannel.EndTryReceiveRequest(result, out context);
    }

    public bool EndWaitForRequest(IAsyncResult result) {
        PrintHelper.Print(_source, "EndWaitForRequest");
        return this.InnerChannel.EndWaitForRequest(result);
    }

    public EndpointAddress LocalAddress {
        get {
            PrintHelper.Print(_source, "LocalAddress");
            return this.InnerChannel.LocalAddress; }
    }

    public RequestContext ReceiveRequest(TimeSpan timeout) {
        PrintHelper.Print(_source, "ReceiveRequest");
        return this.InnerChannel.ReceiveRequest(timeout);
    }

    public RequestContext ReceiveRequest() {
        PrintHelper.Print(_source, "ReceiveRequest, no TimeSpan arg");
        return this.ReceiveRequest(this.DefaultReceiveTimeout);
    }

    public bool TryReceiveRequest(TimeSpan timeout, out RequestContext context) {
        PrintHelper.Print(_source, "TryReceiveRequest");
        return this.InnerChannel.TryReceiveRequest(timeout, out context);
    }

    public bool WaitForRequest(TimeSpan timeout) {
        PrintHelper.Print(_source, "WaitForRequest");
        return this.InnerChannel.WaitForRequest(timeout);
    }

    #endregion
}

internal sealed class DelegatorReplySessionChannel : DelegatorReplyChannel, IReplySessionChannel {

    private IReplySessionChannel _innerSessionChannel;
    private String _source;

    internal DelegatorReplySessionChannel(ChannelManagerBase channelManagerBase, IReplySessionChannel innerChannel, String source)
        : base(channelManagerBase, innerChannel, source) {
    
        _source = String.Format("{0} CHANNEL: DelegatorReplySessionChannel", source);
        PrintHelper.Print(_source, "ctor");
        this._innerSessionChannel = innerChannel;
    }

    #region ISessionChannel<IInputSession> Members

    public IInputSession Session {
        get {
            PrintHelper.Print(_source, "Session");
            return this._innerSessionChannel.Session; }
    }

    #endregion
}
