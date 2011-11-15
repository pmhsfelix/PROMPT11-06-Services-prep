//-----------------------------------------------------------------------------
// WCF Samples and Demos, for educational purposes only
//
// Pedro Félix (pedrofelix at cc.isel.ipl.pt)
// Centro de Cálculo do Instituto Superior de Engenharia de Lisboa
//-----------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace DictionaryClient
{
    // A generic (Message-based) request-response contract
    [ServiceContract]
    public interface IGenericRequestReplyContract
    {
        [OperationContract(
            Action = "*", ReplyAction="*")]
        Message Operation(Message msg);
    }
}
