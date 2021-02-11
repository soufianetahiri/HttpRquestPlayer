
## What is it?
This utility could help you to find authorization bugs. Just edit *request.json* by providing for exmple a high and a low  priviliged user's requests headers, paths to scan and thats it. The tool will proceed to requests with both headers and hilight diffirent http response.

For now, it supports only GET.

## request.json sample

    {
      "baseUrl": "https://www.target.someurl.com",
      "originalHeaders": [
        "Host: www.target.someurl.com",
        "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:85.0) Gecko/20100101 Firefox/85.0",
        "Accept: application/json",
        "Accept-Language: en-US,en;q=0.5",
        "Accept-Encoding: gzip, deflate, br",
        "Referer: https://www.someurl.com",
        "Authorization: Bearer AdminToken",
        "Connection: keep-alive",
        "Cookie: cookie1=random; cookie2=random"
      ],
      "newHeaders": [
        "Host: www.target.someurl.com",
        "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:85.0) Gecko/20100101 Firefox/85.0",
        "Accept: application/json",
        "Accept-Language: en-US,en;q=0.5",
        "Accept-Encoding: gzip, deflate, br",
        "Referer: https://www.someurl.com",
        "Authorization: Bearer SimpleUserToken",
        "Connection: keep-alive",
        "Cookie: cookie1=random2; cookie2=random2"
      ],
      "requests": {
        "get": [
          "/api/v1/users/me/dashboard",
          "/api/v1/campaigns/kpis/chart",
          "/api/v1/users/4623"
        ]
      }
    }

You can add as many headers as you want, the tool will parse and add them to the httpclient automatically.
## Output sample
![enter image description here](https://i.ibb.co/cCmGdzs/Screenshot-2021-02-11-175048.jpg)
## ToDo

 - [ ] Handle exceptions  
 - [ ]  Support more methods (POST PUT DELETE...)  
 - [ ] Parse Burp requests
 - [ ] Add verbosity
