using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MobileLink_Desktop.Utils;

public class ServerAPI
{
    private readonly HttpClient _httpClient;

    public ServerAPI()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("http://localhost:5000");
    }

    public async Task<string> Login(string emailUser, string password)
    {
        var body = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            {
                "emailOrUsername", emailUser
            },
            {
                "password", password
            }
        });
        var response = await _httpClient.PostAsync("/api/Auth/login", body);
        return await response.Content.ReadAsStringAsync();
    }
    public async Task<string> SendCode(string email)
    {
        var body = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            {
                "email", email
            }
        });
        var response = await _httpClient.PostAsync("/api/Auth/sendCode", body);
        return await response.Content.ReadAsStringAsync();
    }
    public async Task<string> VerifyCode(string email, string code)
    {
        var body = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            {
                "email", email
            },
            {
                "code", code
            }
        });
        var response = await _httpClient.PostAsync("/api/Auth/verifyCode", body);
        return await response.Content.ReadAsStringAsync();
    }
    public async Task<string> Register(string email, string password, string username)
    {
        var body = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            {
                "email", email
            },
            {
                "password", password
            },
            {
                "username", username
            }
        });
        var response = await _httpClient.PostAsync("/api/Auth/register", body);
        return await response.Content.ReadAsStringAsync();
    }
}