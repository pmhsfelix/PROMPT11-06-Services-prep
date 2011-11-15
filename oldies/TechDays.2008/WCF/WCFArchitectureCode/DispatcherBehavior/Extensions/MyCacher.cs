using System;
using System.ServiceModel;
using System.Collections.Generic;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

public sealed class MyCacherAttribute : Attribute, IOperationBehavior
{
    public void AddBindingParameters(OperationDescription operationDescription, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
    {
        return;
    }

    public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
    {
        dispatchOperation.Invoker = new MyCacher(dispatchOperation.Invoker);
    }

    public void Validate(OperationDescription operationDescription)
    {
        return;
    }
}

public sealed class MyCacher : IOperationInvoker
{
    public Dictionary<Int32, Int32> cache = new Dictionary<Int32, Int32>();

    public IOperationInvoker innerOperationInvoker;

    public MyCacher(IOperationInvoker innerOperationInvoker)
    {
        this.innerOperationInvoker = innerOperationInvoker;
    }

    public object[] AllocateInputs()
    {
        return this.innerOperationInvoker.AllocateInputs();
    }

    public object Invoke(object instance, object[] inputs, out object[] outputs)
    {
        Int32 key = (Int32) inputs[0];
        Int32 value;

        if(cache.TryGetValue(key, out value))
        {
            outputs = new object[0];
            return value;
        }

        value =  (Int32) this.innerOperationInvoker.Invoke(instance, inputs, out outputs);
        cache[key] = value;
        return value;
    }

    public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
    {
        return this.innerOperationInvoker.InvokeBegin(instance, inputs, callback, state);
    }

    public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
    {
        return this.innerOperationInvoker.InvokeEnd(instance, out outputs, result);
    }

    public bool IsSynchronous
    {
        get { return this.innerOperationInvoker.IsSynchronous; }
    }

}

