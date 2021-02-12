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
                Console.WriteLine(ex.Message);
            }
        }

        static async Task ProcessRequests(RequestModel request)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback +=
                (sender, certificate, chain, errors) =>
                {
                    return true;
                };

            HttpClient client = new HttpClient(clientHandler);
            if (request.requests != null)
            {
                //Process GETs
                if (request.requests.get?.Count > 0)
                {
                    Helper.Write($"Processing {request.requests.get?.Count} GET requests", ConsoleColor.Gray, ConsoleColor.Black);
                    foreach (string reqPath in request.requests.get)
                    {
                        string url = string.Concat(request.baseUrl, reqPath);
                        Console.WriteLine($"{url} =>");
                        ProcessHeaders(request, true, client);
                        HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(true);
                        Console.WriteLine($"Original Headers:  {response.StatusCode} ({(int)response.StatusCode})");
                        ProcessHeaders(request, false, client);
                        HttpResponseMessage response2 = await client.GetAsync(url).ConfigureAwait(true);
                        WriteResults(response, response2);
                    }
                }
                //Process Post
                if (request.requests.withBodies?.post?.bodies?.Count > 0 && request.requests.withBodies?.post?.urls?.Count > 0)
                {
                    Helper.Write($"Processing {request.requests.withBodies.post.bodies.Count} POST requests", ConsoleColor.Gray, ConsoleColor.Black);
                    for (int i = 0; i < request.requests.withBodies.post.bodies.Count; i++)
                    {
                        ProcessHeaders(request, true, client);
                        string reqPath = request.requests.withBodies.post.urls[i];
                        StringContent reqBody = new StringContent(request.requests.withBodies.post.bodies[i], Encoding.UTF8, "application/json");
                        string url = string.Concat(request.baseUrl, reqPath);
                        Console.WriteLine($"{url} =>");
                        ProcessHeaders(request, true, client);
                        HttpResponseMessage response = await client.PostAsync(url, reqBody).ConfigureAwait(true);
                        Console.WriteLine($"Original Headers:  {response.StatusCode} ({(int)response.StatusCode})");
                        ProcessHeaders(request, false, client);
                        HttpResponseMessage response2 = await client.PostAsync(url, reqBody).ConfigureAwait(true);
                        WriteResults(response, response2);
                    }
                }
            }
            client.Dispose();
        }

        private static void WriteResults(HttpResponseMessage response, HttpResponseMessage response2)
        {
            if ((int)response.StatusCode != (int)response2.StatusCode && !response2.IsSuccessStatusCode)
            {
                Helper.Write($"Modified Headers: {response2.StatusCode}({(int)response2.StatusCode})", ConsoleColor.Yellow, ConsoleColor.Black);
            }
            else if ((int)response.StatusCode != (int)response2.StatusCode && response2.IsSuccessStatusCode)
            {
                Helper.Write($"Modified Headers: {response2.StatusCode}({(int)response2.StatusCode})", ConsoleColor.Green, ConsoleColor.Black);
            }
            else
            {
                Console.WriteLine($"Modified Headers: {response2.StatusCode}({(int)response2.StatusCode})");
            }

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
                byte[] bytes = Encoding.ASCII.GetBytes(header[header.IndexOf(":")..].StartsWith(":") ? // in case
                        header[header.IndexOf(":")..].Remove(0, 1) :// a header vakue
                        header[header.IndexOf(":")..]);// contains a ":"
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
