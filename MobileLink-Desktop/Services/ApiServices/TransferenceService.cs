using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MobileLink_Desktop.Classes.Http.Response;
using MobileLink_Desktop.Entities;
using MobileLink_Desktop.Utils;

namespace MobileLink_Desktop.Service.ApiServices;

public class TransferenceService(ServerAPI api)
{
    public async Task<int?> StartTransference(int idDevice, string filePath, string fileNameExtension, long fileSize, string destinationPath)
    {
        var content = new StringContent(JsonSerializer.Serialize(new
        {
            idDevice,
            fileSize,
            filePath,
            fileNameExtension,
            destinationPath
        }), Encoding.UTF8, "application/json");
        var response = await api.HttpClient.PostAsync("/api/Transfer/startTransference", content);//TODO send token, or somehow get the user devices only
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
        var response = await api.HttpClient.PostAsync("/api/Transfer/sendFileChunk", content);
        if (!response.IsSuccessStatusCode)
        {
            //Todo popup or return error
            return false;
        }
        return true;
    }
    public async Task<Transference?> GetTransfer(int idTransfer)
    {
        var response = await api.HttpClient.GetAsync($"/api/Transfer/getTransfer?idTransfer={idTransfer}");
        if (!response.IsSuccessStatusCode)
        {
            //Todo popup or return error
            return null;
        }
        var body = await response.Content.ReadAsStringAsync();
        var deserialized = body.DeserializeFromCamelCase<Transference>();
        return deserialized;
    }
    
    public async Task<bool> FinishTransfer(int idTransfer)
    {
        var content = new StringContent(JsonSerializer.Serialize(new
        {
            idTransfer
        }), Encoding.UTF8, "application/json");
        var response = await api.HttpClient.PostAsync($"/api/Transfer/finishTransfer", content);
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }
        var body = await response.Content.ReadAsStringAsync();
        return body == "true";
    }
    
    public async Task<TransferenceChunk?> GetChunkWithTransference(int idChunk)
    {
        var response = await api.HttpClient.GetAsync($"/api/Transfer/getChunkWithTransference?idChunk={idChunk}");
        if (!response.IsSuccessStatusCode)
        {
            //Todo popup or return error
            return null;
        }
        var body = await response.Content.ReadAsStringAsync();
        var deserialized = body.DeserializeFromCamelCase<TransferenceChunk>();
        return deserialized;
    }
    
    public async Task<GetChunkBytesResponse?> GetChunkBytes(int idChunk)
    {
        var response = await api.HttpClient.GetAsync($"/api/Transfer/getChunkBytes?idChunk={idChunk}");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        var body = await response.Content.ReadAsStringAsync();
        var deserialized = body.DeserializeFromCamelCase<GetChunkBytesResponse>();
        return deserialized;
    }
    
    public async Task<List<Transference>> GetTransfersNotOnServer()
    {
        var response = await api.HttpClient.GetAsync($"/api/Transfer/getTransfersNotOnServer");
        if (!response.IsSuccessStatusCode)
        {
            //Todo popup or return error
            return [];
        }
        var body = await response.Content.ReadAsStringAsync();
        var deserialized = body.DeserializeFromCamelCase<List<Transference>>();
        return deserialized ?? [];
    }
    
    public async Task<List<Transference>> GetTransfersNotOnDestination()
    {
        var response = await api.HttpClient.GetAsync($"/api/Transfer/getTransfersNotOnDestination");
        if (!response.IsSuccessStatusCode)
        {
            //Todo popup or return error
            return [];
        }
        var body = await response.Content.ReadAsStringAsync();
        var deserialized = body.DeserializeFromCamelCase<List<Transference>>();
        return deserialized ?? [];
    }
    
    public async Task<List<TransferenceChunk>?> GetTransferChunks(int idTransfer)
    {
        var response = await api.HttpClient.GetAsync($"/api/Transfer/getTransferChunks?idTransfer={idTransfer}");
        if (!response.IsSuccessStatusCode)
        {
            //Todo popup or return error
            return null;
        }
        var body = await response.Content.ReadAsStringAsync();
        return body.DeserializeFromCamelCase<List<TransferenceChunk>>();
    }
    //TODO check necessity
    public async Task<bool> CheckTransferChunksCompletion(int idTransfer)
    {
        var response = await api.HttpClient.GetAsync($"/api/Transfer/getTransfer?idTransfer={idTransfer}");
        if (!response.IsSuccessStatusCode)
        {
            //Todo popup or return error
            return false;
        }
        var body = await response.Content.ReadAsStringAsync();
        return body == "true";
    }

}