using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using MobileLink_Desktop.Utils;

namespace MobileLink_Desktop.Service.ApiServices;

public class ConnectionService(ServerAPI api)
{
    public async Task<List<int>> GetConnectedDevices()
    {
        var response = await api.HttpClient.GetAsync("/api/Connection/connections?userId=1");//TODO send token, or somehow get the user devices only
        var resContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            //Todo popup or return error
            return [];
        }
        return JsonSerializer.Deserialize<List<int>>(resContent) ?? [];
    }
}