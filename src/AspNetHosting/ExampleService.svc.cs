using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;

namespace AspNetHosting
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ExampleService" in code, svc and config file together.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ExampleService : IExampleService
    {
        public string ToUpper(string s)
        {
            return s.ToUpper();
        }
    }
}
