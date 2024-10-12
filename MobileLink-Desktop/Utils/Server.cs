using System;
using System.Net.Http;

namespace MobileLink_Desktop.Utils;

public class Server
{
    private readonly HttpClient _httpClient;

    public Server()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("http://localhost:5000"); // Set your API endpoint
    }
}