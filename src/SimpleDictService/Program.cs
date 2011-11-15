using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;

namespace SimpleDictService
{
    [ServiceContract]
    public interface ITheServiceContract
    {
        [OperationContract]
        WordLookupResp WordLookup(WordLookupReq Request);
    }

    [DataContract]
    public class WordLookupReq
    {
        [DataMember]
        public string Word { get; set; }
    }

    [DataContract]
    public class WordLookupResp
    {
        [DataMember]
        public bool Exists { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Definition { get; set; }
    }

    internal class TheServiceImpl : ITheServiceContract
    {

        private static readonly IDictionary<string, string> _map
            = new Dictionary<string, string>
             {
                 {
                     "professor",
                     "Aquele que ensina uma arte, uma actividade, uma ciência, uma língua, etc."
                 }
             };

        public WordLookupResp WordLookup(WordLookupReq req)
        {
            string def;
            return _map.TryGetValue(req.Word, out def) ?
                new WordLookupResp { Exists = true, Definition = def }
                : new WordLookupResp { Exists = false };
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(TheServiceImpl), new Uri("http://localhost:8080")))
            {
                host.AddServiceEndpoint(typeof(ITheServiceContract), new BasicHttpBinding(), "dict");
                host.Description.Behaviors.Add(
                    new ServiceMetadataBehavior()
                        {
                            HttpGetEnabled = true,
                            HttpGetUrl = new Uri("http://localhost:8080/metadata")
                        });
                host.Open();
                Console.WriteLine("Host is opened, press any key to end ...");
                UseTheService();
                Console.ReadKey();
            }
        }


        static void UseTheService()
        {
            using (var ch = new ChannelFactory<ITheServiceContract>(new BasicHttpBinding(), "http://localhost:8080/dict"))
            {
                var channel = ch.CreateChannel();
                var resp = channel.WordLookup(new WordLookupReq {Word = "professor"});
                Console.WriteLine(resp.Exists ? resp.Definition : "not found");
            }
        }
    }
}
