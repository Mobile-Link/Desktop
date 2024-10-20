using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MobileLink_Desktop.Service.Interceptors;

namespace MobileLink_Desktop.Utils;

public class ServerAPI
{
    public readonly HttpClient HttpClient;

    public ServerAPI()
    {
        HttpClient = new HttpClient(new TokenInterceptor());
        HttpClient.BaseAddress = new Uri("http://localhost:5000");
    }
}