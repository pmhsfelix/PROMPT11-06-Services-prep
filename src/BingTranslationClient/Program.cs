using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BingTranslationClient.Bing.Translator;

namespace BingTranslationClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var svc = new LanguageServiceClient();
            var appId = "C1E6D88CE2967328BBA9BC6C932B9D177247CAE5";
            var appIdToken = svc.GetAppIdToken(appId, 5, 1, 60);
            Console.WriteLine(appIdToken);
            var resp = svc.GetTranslations(appId, "hello", "en", "pt", 1, new TranslateOptions());
            foreach(var t in resp.Translations)
            {
                Console.WriteLine(t.TranslatedText);
            }
        }
    }
}
