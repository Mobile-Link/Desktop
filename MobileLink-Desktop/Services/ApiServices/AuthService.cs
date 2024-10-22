using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using MobileLink_Desktop.Classes.Http.Request;
using MobileLink_Desktop.Classes.Http.Response;
using MobileLink_Desktop.Utils;

namespace MobileLink_Desktop.Service.ApiServices;

public class AuthService(ServerAPI api)
{
    public async Task<List<int>> Register()
    {
        var response =
            await api.HttpClient.GetAsync(
                "/api/Connection/connections"); //TODO send token, or somehow get the user devices only
        var resContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            //Todo popup or return error
            return [];
        }

        return JsonSerializer.Deserialize<List<int>>(resContent) ?? [];
    }

    public async Task<HttpResponseMessage> ValidateCredentials(string emailUser, string password)
    {
        var content = new StringContent(JsonSerializer.Serialize(new
        {
            emailOrUsername = emailUser,
            password = password,
        }), Encoding.UTF8, "application/json");
        return await api.HttpClient.PostAsync("/api/Auth/validateCredentials", content);
    }
    
    public async Task<bool> VerifyToken()
    {
        try
        {
            var result = await api.HttpClient.GetAsync("/api/Auth/verifyToken");
            return result.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            //TODO popup or return error
            return false;
        }
    }

    public async Task<RegisterResponse?> LoginCreateDevice(string emailUser, string password, string deviceName,
        string code)
    {
        var content = new StringContent(JsonSerializer.Serialize(new
        {
            emailOrUsername = emailUser,
            password,
            code,
            deviceName
        }), Encoding.UTF8, "application/json");

        var response = await api.HttpClient.PostAsync("/api/Auth/loginCreateDevice", content);
        var body = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            return JsonSerializer.Deserialize<RegisterResponse>(body);
        }

        return new RegisterResponse();
    }

    public async Task<RegisterResponse?> Login(string emailUser, string password, int idDevice)
    {
        var content = new StringContent(JsonSerializer.Serialize(new
        {
            emailOrUsername = emailUser,
            password,
            idDevice
        }), Encoding.UTF8, "application/json");

        var response = await api.HttpClient.PostAsync("/api/Auth/login", content);
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<RegisterResponse>(body);
    }

    public async Task<string> SendCode(string email)
    {
        var body = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            // {
            //     "email", email
            // }
        });
        var response = await api.HttpClient.PostAsync($"/api/Auth/sendCode?email={email}", body);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<bool> VerifyCode(string emailOrUsername, string code)
    {
        var content = new StringContent(JsonSerializer.Serialize(new
            {
                // email = emailOrUsername, code
            }), Encoding.UTF8,
            "application/json");

        var response = await api.HttpClient.PostAsync($"/api/Auth/verifyCode?email={emailOrUsername}&code={code}", content);
        await response.Content.ReadAsStringAsync();
        return response.IsSuccessStatusCode;
    }

    public async Task<RegisterResponse?> Register(RegisterRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await api.HttpClient.PostAsync("/api/Auth/register", content);
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<RegisterResponse>(body);
    }
}