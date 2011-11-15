//-----------------------------------------------------------------------------
// Samples and Demos, for educational purposes only
//
// Pedro Félix (pedrofelix at cc.isel.ipl.pt) 
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Microsoft.ServiceBus;
using System.Net;
using System.ServiceModel.Channels;
using Credentials;

namespace GetScreen {

    [ServiceContract]
    [ServiceBehavior(
        ConcurrencyMode = ConcurrencyMode.Single, 
        InstanceContextMode = InstanceContextMode.Single)]  
    class ScreenResource {       

        private byte[] imageBuffer;
        private DateTime timeStamp;

        private ImageCodecInfo GetEncoder(ImageFormat format) {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs) {
                if (codec.FormatID == format.Guid) {
                    return codec;
                }
            }
            return null;
        }
        private void GenerateImageBuffer(){
            Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            MemoryStream ms = new MemoryStream();

            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
            System.Drawing.Imaging.Encoder myEncoder =
                System.Drawing.Imaging.Encoder.Quality;        
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 5L);
            myEncoderParameters.Param[0] = myEncoderParameter;

            bitmap.Save(ms, jgpEncoder, myEncoderParameters);
            imageBuffer = ms.GetBuffer();
            timeStamp = DateTime.Now;
        }

        [OperationContract]
        [WebGet(UriTemplate = "/screen")]
        Stream Get() {

            // Capture new screen or used cached version?
            Console.WriteLine("Handling request");
            if (imageBuffer == null || DateTime.Now.Subtract(timeStamp).Minutes >= 1) {
                Console.WriteLine("  Generating new image buffer");
                GenerateImageBuffer();
            } else {
                Console.WriteLine("  Returning cached image, age {0}", DateTime.Now.Subtract(timeStamp));
            }

            // Set response HTTP headers
            WebOperationContext.Current.OutgoingResponse.ContentType = "image/jpeg";
            HttpResponseHeader cacheHeader = HttpResponseHeader.CacheControl;
            String cacheControlValue = "public, max-age=60, s-maxage=60";
            WebOperationContext.Current.OutgoingResponse.Headers.Add(cacheHeader, cacheControlValue);            

            // Return image buffer
            return new MemoryStream(imageBuffer);            
        }
    }

    class Program {

        static void Main(string[] args) {
            using (var host = new WebServiceHost(typeof(ScreenResource))) {

                host.AddServiceEndpoint(
                    typeof(ScreenResource),
                    new WebHttpRelayBinding(EndToEndWebHttpSecurityMode.None, RelayClientAuthenticationType.None),
                    "http://felixdemos.servicebus.windows.net/rest");

                host.AddServiceEndpoint(
                    typeof(ScreenResource),
                    new WebHttpBinding(),
                    "http://localhost:8080/rest");

                CredentialsHelper.ConfigureServiceCredentials(host);
                host.Open();
                Console.WriteLine("Service opened, press any key to continue");
                Console.ReadKey();
            }
        }
    }
}
