using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpRquestPlayer
{
    //TODO handle exceptions
    class Program
    {
        private const string _get = "get";
        static async Task Main()
        {
            PrintBanner();
            try
            {
                RequestModel requestModel = JsonConvert.DeserializeObject<RequestModel>(File.ReadAllText(@"..\\..\\..\\requests.json"));
                if (requestModel != null)
                {
                    await ProcessRequests(requestModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
        }

        static async Task ProcessRequests(RequestModel request)
        {
            HttpClient client = new HttpClient();

            if (request.requests?.Count > 0)
            {
                foreach (KeyValuePair<string, List<string>> keyValuePair in request.requests)
                {
                    switch (keyValuePair.Key)
                    {
                        //TODO add other methods (POST PUT DELETE...)
                        case _get:
                            if (keyValuePair.Value?.Count > 0)
                            {
                                foreach (string reqPath in keyValuePair.Value)
                                {

                                    string url = string.Concat(request.baseUrl, reqPath);
                                    Console.WriteLine($"Testing URL {url} :");
                                    ProcessHeaders(request, true, client);
                                    HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(true);
                                    Console.WriteLine($"Original Headers:  {response.StatusCode} ({(int)response.StatusCode})");
                                    ProcessHeaders(request, false, client);
                                    HttpResponseMessage response2 = await client.GetAsync(url).ConfigureAwait(true);
                                    if ((int)response.StatusCode != (int)response2.StatusCode)
                                    {
                                        Console.BackgroundColor = ConsoleColor.Yellow;
                                        Console.ForegroundColor = ConsoleColor.Black;
                                    }
                                    Console.WriteLine($"Modified Headers: {response2.StatusCode}({(int)response2.StatusCode})");
                                    Console.ResetColor();
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            client.Dispose();
        }

        private static void ProcessHeaders(RequestModel request, bool isOriginalRequest, HttpClient client)
        {
            client.DefaultRequestHeaders.Clear();
            if (isOriginalRequest && request.originalHeaders?.Count > 0)
            {
                foreach (string header in request.originalHeaders)
                {
                    Helper.AddHeader(client, header.Split(":")[0], GetHeaderValue(header));
                }
            }
            else if (!isOriginalRequest && request.newHeaders?.Count > 0)
            {
                foreach (string header in request.newHeaders)
                {
                    Helper.AddHeader(client, header.Split(":")[0], GetHeaderValue(header));
                }
            }

            static string GetHeaderValue(string header)
            {
                //Request headers must contain only ASCII characters
                byte[] bytes = Encoding.ASCII.GetBytes(header.Substring(header.IndexOf(":")).StartsWith(":") ? // in case
                        header.Substring(header.IndexOf(":")).Remove(0, 1) :// a header vakue
                        header.Substring(header.IndexOf(":")));// contains a ":"
                return Encoding.ASCII.GetString(bytes);
            }
        }
        private static void PrintBanner()
        {
            Console.SetWindowSize(
 Math.Min(140, Console.LargestWindowWidth),
 Math.Min(50, Console.LargestWindowHeight));
            Console.Title = "HttpRequestPlayer v0.1";
            string banner = @"     ___ ___  __   __   ___  __        ___  __  ___     __                 ___  __  
|__|  |   |  |__) |__) |__  /  \ |  | |__  /__`  |     |__) |     /\  \ / |__  |__) 
|  |  |   |  |    |  \ |___ \__X \__/ |___ .__/  |     |    |___ /~~\  |  |___ |  \ 
                                                                                    ";
            Console.WriteLine(banner);
        }
    }
}
