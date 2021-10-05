using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

using System.Text;


namespace coreapp
{

    abstract class HttpRequestRW {

        public abstract void setup(string uri);
        public virtual async Task<string> SendCommandAndRecieveResult(string line){
            return null;
        }


    }

    class PostHttpRequestRW : HttpRequestRW {

        Uri recieverUri = null;
        string textRecieverUri;

        public override void setup(string uri) {
            this.textRecieverUri = uri;
            recieverUri = new Uri(textRecieverUri);
            
        }

        public override async Task<string> SendCommandAndRecieveResult(string line) {
            
            

            HttpClient client = new HttpClient();
            
            client.BaseAddress = recieverUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.Timeout = TimeSpan.FromSeconds(3);
            Console.WriteLine("command: " + line);
            var request = new HttpRequestMessage(HttpMethod.Post, textRecieverUri);
            request.Content = new StringContent(line, Encoding.UTF8);
            //request.Content = new StringContent("commandRequest=" + line, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");


            HttpResponseMessage response = null;
            try
            {
                response = await client.SendAsync(request);
                System.Diagnostics.Debug.WriteLine("Response:");
                System.Diagnostics.Debug.WriteLine(response.StatusCode);
                if (response.IsSuccessStatusCode)
                {

                    var content = await response.Content.ReadAsStringAsync();


                    //int idx1 = content.IndexOf("<body>") + "<body>".Length + 2;
                    //int idx2 = content.IndexOf("</body>");


                    //return content.Substring(idx1, idx2 - idx1);
                    return content;
                }
                else
                {
                    return null;
                }
            } catch(Exception e) {
                System.Diagnostics.Debug.WriteLine("Message:");
                System.Diagnostics.Debug.WriteLine(e.Message);
                WpfApp2.EditorPrinter.PrintStatus("Connection error");

            }

            //response.EnsureSuccessStatusCode();

            return null;
           

        }

    }


}
