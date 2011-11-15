using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace AspNetHosting
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        string ToUpper(string s);
    }
}