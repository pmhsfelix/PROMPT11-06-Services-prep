using System;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;

public class MyValidatorAttribute : Attribute, IOperationBehavior
{
    public Int32 Index;
    public Int32 Min = 0;
    public Int32 Max = 100;

    public void ApplyBehavior(OperationDescription description, DispatchOperation dispatch, BindingParameterCollection parameters)
    {
        dispatch.ParameterInspectors.Add(new MyValidator(Index, Max, Min));
        dispatch.FaultContractInfos.Add(new FaultContractInfo("Validation Error", typeof(String)));
    }

    public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
    {
        return;
    }

    public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
    {
        clientOperation.ParameterInspectors.Add(new MyValidator(Index, Max, Min));
        clientOperation.FaultContractInfos.Add(new FaultContractInfo("Validation Error", typeof(String)));
    }

    public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
    {
        dispatchOperation.ParameterInspectors.Add(new MyValidator(Index, Max, Min));
        dispatchOperation.FaultContractInfos.Add(new FaultContractInfo("Validation Error", typeof(String)));
    }

    public void Validate(OperationDescription operationDescription)
    {
        return;
    }

}

public class MyValidator : IParameterInspector
{
    Int32 Index;
    Int32 min;
    Int32 max;

    public MyValidator(Int32 index, Int32 max, Int32 min)
    {
        this.Index = index;
        this.max = max;
        this.min = min;
    }

    public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
    {

    }

    public object BeforeCall(string operationName, object[] inputs)
    {
        Int32 n = (Int32)inputs[Index];
        if ((n < min) || (n > max))
        {
            throw new FaultException<string>("Argument out of range");
        }

        return null;
    }

}