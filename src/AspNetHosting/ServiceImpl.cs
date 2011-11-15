using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetHosting
{
    internal class ServiceImpl : IService
    {
        public string ToUpper(string s)
        {
            return s.ToUpper();
        }
    }
}