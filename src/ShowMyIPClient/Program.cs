using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShowMyIPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new MyIp.Reference.callshowmyip_lookupPortTypeClient();
            var resp = client.callshowmyip_lookup("73.198.210.194", null, null, null, null,null,null);
            Console.WriteLine("City = {0}",resp.CITY);

        }
    }
}
