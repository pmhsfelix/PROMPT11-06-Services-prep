using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Channels;

public sealed class DelegatorBindingElement : BindingElement
{
    public override bool CanBuildChannelFactory<TShape>(BindingContext context)
    {
        return context.CanBuildInnerChannelFactory<TShape>();
    }

    public override bool CanBuildChannelListener<TShape>(BindingContext context)
    {
        return context.CanBuildInnerChannelListener<TShape>();
    }

    public override IChannelFactory<TShape> BuildChannelFactory<TShape>(BindingContext context)
    {
        if(!this.CanBuildChannelFactory<TShape>(context))
        {
            throw new InvalidOperationException("Unsupported channel type");
        }
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }

        DelegatorChannelFactory<TShape> factory = new DelegatorChannelFactory<TShape>(context);
        return (IChannelFactory<TShape>) factory;
    }

    public override IChannelListener<TShape> BuildChannelListener<TShape>(BindingContext context)
    {
        if(!this.CanBuildChannelListener<TShape>(context))
        {
            throw new InvalidOperationException("Unsupported channel type");
        }

        if (context == null)
        {
            throw new ArgumentNullException("context");
        }

        DelegatorChannelListener<TShape> listener = new DelegatorChannelListener<TShape>(context);
        return (IChannelListener<TShape>) listener;    
    }

    public override BindingElement Clone()
    {
        return new DelegatorBindingElement();
    }

    public override T GetProperty<T>(BindingContext context)
    {
        return context.GetInnerProperty<T>();
    }
}

