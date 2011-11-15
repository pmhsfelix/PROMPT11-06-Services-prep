using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;

internal sealed class DelegatorChannelFactory<TShape> : ChannelFactoryBase<TShape>
{
    // reference the next channel factory in the stack
    private IChannelFactory<TShape> _innerFactory;

    // the String to print to the console
    private String _consolePrefix = "FACTORY: DelegatorChannelFactory";

    // ctor that builds the next channel factory in the stack,
    // then assigns the _innerFactory member variable
    // calls the base ctor to set timeouts
    internal DelegatorChannelFactory(BindingContext context) : base(context.Binding) {
        PrintHelper.Print(_consolePrefix, "ctor");
        this._innerFactory = context.BuildInnerChannelFactory<TShape>();
    }

    // instantiates and returns a DelegatorChannel that
    // references another channel
    private TShape WrapChannel(TShape innerChannel) {
        
        if(innerChannel == null) {
            throw new ArgumentNullException("innerChannel cannot be null", "innerChannel");
        }

        // Go through the channel shapes and return a wrapped channel
        if(typeof(TShape) == typeof(IOutputChannel)) {
            return (TShape)(object)new DelegatorOutputChannel<IOutputChannel>(this, (IOutputChannel)innerChannel, "SEND");
        }
        
        if(typeof(TShape) == typeof(IRequestChannel)) {
            return (TShape)(object)new DelegatorRequestChannel(this, (IRequestChannel)innerChannel, "SEND");
        }
        
        if(typeof(TShape) == typeof(IDuplexChannel)) {
            return (TShape)(object)new DelegatorDuplexChannel(this, (IDuplexChannel)innerChannel, "SEND");
        }
        
        if(typeof(TShape) == typeof(IOutputSessionChannel)) {
            return (TShape)(object)new DelegatorOutputSessionChannel(this, (IOutputSessionChannel)innerChannel, "SEND");
        }
        
        if(typeof(TShape) == typeof(IRequestSessionChannel)) {
            return (TShape)(object)new DelegatorRequestSessionChannel(this, (IRequestSessionChannel)innerChannel, "SEND");
        }
        
        if(typeof(TShape) == typeof(IDuplexSessionChannel)) {
            return (TShape)(object)new DelegatorDuplexSessionChannel(this, (IDuplexSessionChannel)innerChannel, "SEND");
        }

        // Cannot wrap this channel.
        throw new ArgumentException(String.Format("invalid channel shape passed:{0}", innerChannel.GetType()));
    }

    // uses the _innerFactory member variable to build a channel
    // then wraps it and returns the wrapped channel
    protected override TShape OnCreateChannel(EndpointAddress address, Uri via)
    {
        // create and return the channel
        PrintHelper.Print(_consolePrefix, "OnCreateChannel");
        TShape innerChannel = this._innerFactory.CreateChannel(address, via);
        return WrapChannel(innerChannel);
    }


    protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
    {
        PrintHelper.Print(_consolePrefix, "OnBeginChannel");
        return this._innerFactory.BeginOpen(timeout, callback, state);
    }

    protected override void OnAbort() {
        base.OnAbort();
        PrintHelper.Print(_consolePrefix, "OnAbort");
    }

    protected override void OnClose(TimeSpan timeout) {
        base.OnClose(timeout);
        PrintHelper.Print(_consolePrefix, "OnClose");
    }

    protected override void OnEndOpen(IAsyncResult result)
    {
        PrintHelper.Print(_consolePrefix, "OnEndOpen");
        this._innerFactory.EndOpen(result);
    }

    protected override void OnOpen(TimeSpan timeout)
    {
        PrintHelper.Print(_consolePrefix, "OnOpen");
        this._innerFactory.Open(timeout);
    }

    public override T GetProperty<T>()
    {
        PrintHelper.Print(_consolePrefix, "GetProperty<" + typeof(T).Name + ">");
        return this._innerFactory.GetProperty<T>();
    }
}

