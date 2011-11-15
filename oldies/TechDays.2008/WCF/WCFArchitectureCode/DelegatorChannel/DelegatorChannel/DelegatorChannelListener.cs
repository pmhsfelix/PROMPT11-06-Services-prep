using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;


internal sealed class DelegatorChannelListener<TShape> : ChannelListenerBase<TShape> where TShape : class, IChannel {
    // reference to tne next channel listener in the stack
    private IChannelListener<TShape> _innerListener; // 

    // the string to print to the console
    String _consolePrefix = "LISTENER: DelegatorChannelListener";
    
    // build the next channel listener, then refer to it
    // calls the base ctor to set timeouts
    public DelegatorChannelListener(BindingContext context) : base(context.Binding) {
        this._innerListener = context.BuildInnerChannelListener<TShape>();
    }

    // instantiates and returns a DelegatorChannel that
    // references another channel
    private TShape WrapChannel(TShape innerChannel) {
        
        if(innerChannel == null) {
            throw new ArgumentNullException("innerChannel cannot be null", "innerChannel");
        }

        // Go through the channel shapes and return a wrapped channel
        if(typeof(TShape) == typeof(IInputChannel)) {
            return (TShape)(object)new DelegatorInputChannel<IInputChannel>(this, (IInputChannel)innerChannel, "RECEIVE");
        }
        if (typeof(TShape) == typeof(IReplyChannel)) {
            return (TShape)(object)new DelegatorReplyChannel(this, (IReplyChannel)innerChannel, "RECEIVE");
        }
        if (typeof(TShape) == typeof(IDuplexChannel)) {
            return (TShape)(object)new DelegatorDuplexChannel(this, (IDuplexChannel)innerChannel, "RECEIVE");
        }
        if (typeof(TShape) == typeof(IInputSessionChannel)) {
            return (TShape)(object)new DelegatorInputSessionChannel(this, (IInputSessionChannel)innerChannel, "RECEIVE");
        }
        if (typeof(TShape) == typeof(IReplySessionChannel)) {
            return (TShape)(object)new DelegatorReplySessionChannel(this, (IReplySessionChannel)innerChannel, "RECEIVE");
        }
        if (typeof(TShape) == typeof(IDuplexSessionChannel)) {
            return (TShape)(object)new DelegatorDuplexSessionChannel(this, (IDuplexSessionChannel)innerChannel, "RECEIVE");
        }

        // Cannot wrap this channel.
        throw new ArgumentException(String.Format("invalid channel shape passed:{0}", innerChannel.GetType()));
    }

    protected override TShape OnAcceptChannel(TimeSpan timeout)
    {
        // create and return the channel
        PrintHelper.Print(_consolePrefix, "OnAcceptChannel");
        TShape innerChannel = _innerListener.AcceptChannel(timeout);
        
        // when shutting down, the innerChannel can be null
        if (innerChannel != null) {
            return WrapChannel(innerChannel);
        }

        return null;
    }

    protected override IAsyncResult OnBeginAcceptChannel(TimeSpan timeout, AsyncCallback callback, object state)
    {
        PrintHelper.Print(_consolePrefix, "OnBeginAcceptChannel");
        return this._innerListener.BeginAcceptChannel(timeout, callback, state);
    }

    protected override TShape OnEndAcceptChannel(IAsyncResult result)
    {
        // create and return the channel
        PrintHelper.Print(_consolePrefix, "OnEndAcceptChannel");
        TShape innerChannel = _innerListener.EndAcceptChannel(result);
        // when closing, _inner.EndAcceptChannel returns null, nothing to wrap
        if (innerChannel != null) {
            return WrapChannel(innerChannel);
        }
        return null;
    }

    protected override IAsyncResult OnBeginWaitForChannel(TimeSpan timeout, AsyncCallback callback, object state)
    {
        PrintHelper.Print(_consolePrefix, "OnBeginWaitForChannel");
        return this._innerListener.BeginWaitForChannel(timeout, callback, state);
    }

    protected override bool OnEndWaitForChannel(IAsyncResult result)
    {
        PrintHelper.Print(_consolePrefix, "OnEndWaitForChannel");
        return this._innerListener.EndWaitForChannel(result);
    }

    protected override bool OnWaitForChannel(TimeSpan timeout)
    {
        PrintHelper.Print(_consolePrefix, "OnWaitForChannel");
        return this._innerListener.WaitForChannel(timeout);
    }

    public override Uri Uri
    {
        get {
            PrintHelper.Print(_consolePrefix, "Uri");
            return this._innerListener.Uri; 
        }
    }

    protected override void OnAbort()
    {
        PrintHelper.Print(_consolePrefix, "OnAbort");
        this._innerListener.Abort();
    }

    protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
    {
        PrintHelper.Print(_consolePrefix, "OnBeginClose");
        return this._innerListener.BeginClose(timeout, callback, state);
    }

    protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
    {
        PrintHelper.Print(_consolePrefix, "OnBeginOpen");
        return this._innerListener.BeginOpen(timeout, callback, state);
    }

    protected override void OnClose(TimeSpan timeout)
    {
        PrintHelper.Print(_consolePrefix, "OnClose");
        this._innerListener.Close(timeout);
    }

    protected override void OnEndClose(IAsyncResult result)
    {
        PrintHelper.Print(_consolePrefix, "OnEndClose");
        this._innerListener.EndClose(result);
    }

    protected override void OnEndOpen(IAsyncResult result)
    {
        PrintHelper.Print(_consolePrefix, "OnEndOpen");
        this._innerListener.EndOpen(result);
    }

    protected override void OnOpen(TimeSpan timeout)
    {
        PrintHelper.Print(_consolePrefix, "OnOpen");
        this._innerListener.Open(timeout);
    }

    public override T GetProperty<T>()
    {
        PrintHelper.Print(_consolePrefix, "GetProperty<" + typeof(T) + ">");
        return this._innerListener.GetProperty<T>();
    }
}


