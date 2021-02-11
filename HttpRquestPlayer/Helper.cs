using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace HttpRquestPlayer
{
    public static class Helper
    {

    

        public static HttpClient AddHeader(HttpClient client, string headerName, string headerValue)
        {
            client.DefaultRequestHeaders.Add(headerName, headerValue);
            return client;
        }
    }
}
