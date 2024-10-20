using System.Collections.Generic;
using System.Threading.Tasks;
using MobileLink_Desktop.Utils;
using System.Text.Json;
using MobileLink_Desktop.Entities;

namespace MobileLink_Desktop.Service.ApiServices;

public class DeviceService(ServerAPI api)
{
    public async Task<List<Device>> GetUserDevices()
    {
        var res = await api.HttpClient.GetAsync("/api/Device/GetUserDevices");//TODO remove param
        var resContent = await res.Content.ReadAsStringAsync();
        if (!res.IsSuccessStatusCode)
        {
            //Todo popup or return error
            return [];
        }
        return JsonSerializer.Deserialize<List<Device>>(resContent) ?? [];
    }
}