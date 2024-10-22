using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MobileLink_Desktop.Utils;

namespace MobileLink_Desktop.Service.ApiServices;

public class TransferenceService(ServerAPI api)
{
    public async Task<int?> StartTransference(int idDevice, string filePath, long fileSize, string destinationPath)
    {
        var content = new StringContent(JsonSerializer.Serialize(new
        {
            idDevice,
            fileSize,
            filePath,
            destinationPath
        }), Encoding.UTF8, "application/json");
        var response = await api.HttpClient.PostAsync("/api/Transfer/StartTransference", content);//TODO send token, or somehow get the user devices only
        var resContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            //Todo popup or return error
            return null;
        }
        return int.Parse(resContent);
    }
    public async Task<bool> SendFileChunk(int idTransfer, long startByteIndex, byte[] byteArray)
    {
        var content = new StringContent(JsonSerializer.Serialize(new
        {
            idTransfer,
            startByteIndex,
            byteArray
        }), Encoding.UTF8, "application/json");
        var response = await api.HttpClient.PostAsync("/api/Transfer/SendFileChunk", content);
        if (!response.IsSuccessStatusCode)
        {
            //Todo popup or return error
            return false;
        }
        return true;
    }
}