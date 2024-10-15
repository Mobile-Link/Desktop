using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using MobileLink_Desktop.Classes.Http.Request;
using MobileLink_Desktop.Utils;

namespace MobileLink_Desktop.Service.ApiServices;

public class AuthService(ServerAPI api)
{
    public async Task<List<int>> Register()
    {
        var response = await api.HttpClient.GetAsync("/api/Connection/connections");//TODO send token, or somehow get the user devices only
        var resContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            //Todo popup or return error
            return [];
        }
        return JsonSerializer.Deserialize<List<int>>(resContent) ?? [];
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
        var response = await api.HttpClient.PostAsync("/api/Auth/login", body);
        return await response.Content.ReadAsStringAsync();
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
    public async Task<bool> VerifyCode(string email, string code)
    {
        var body = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            // {
            //     "email", email
            // },
            // {
            //     "code", code
            // }
        });
        var response = await api.HttpClient.PostAsync($"/api/Auth/verifyCode?email={email}&code={code}", body);
        await response.Content.ReadAsStringAsync();
        return response.IsSuccessStatusCode;
    }
    public async Task<string> Register(RegisterRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await api.HttpClient.PostAsync("/api/Auth/register", content);
        return await response.Content.ReadAsStringAsync();
    }
}