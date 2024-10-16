using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MobileLink_Desktop.Utils;

public class ServerAPI
{
    public readonly HttpClient HttpClient;

    public ServerAPI()
    {
        HttpClient = new HttpClient();
        HttpClient.BaseAddress = new Uri("http://localhost:5000");
    }
}