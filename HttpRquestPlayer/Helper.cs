using System;
using System.Net.Http;

namespace HttpRquestPlayer
{
    public static class Helper
    {
        public static HttpClient AddHeader(HttpClient client, string headerName, string headerValue)
        {
            client.DefaultRequestHeaders.Add(headerName, headerValue);
            return client;
        }
        public static void Write(string text, ConsoleColor background, ConsoleColor foreground )
        {
            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
