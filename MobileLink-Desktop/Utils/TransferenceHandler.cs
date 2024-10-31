using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MobileLink_Desktop.Entities;
using MobileLink_Desktop.Enums;
using MobileLink_Desktop.Service.ApiServices;

namespace MobileLink_Desktop.Utils;

public class TransferenceHandler(TransferenceService transferenceService)
{
    public async Task ReceiveFileChunk(int idTransfer, long startByteIndex, byte[] byteArray)
    {
        Console.WriteLine($"New chunk received {idTransfer}, {startByteIndex}, Length: {byteArray.Length}");
        var localStorage = new LocalStorage();
        var localStorageContent = localStorage.GetStorage();
        if (localStorageContent == null)
        {
            return;
        }
        var transference = await transferenceService.GetTransfer(idTransfer);
        if (transference == null)
        {
            return;
        }

        var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "MobileLink",transference.IdTransference.ToString());
        var chunkPath = Path.Combine(directory, $"{startByteIndex}.bin");
        Directory.CreateDirectory(directory);
        await File.WriteAllBytesAsync(chunkPath, byteArray);//TODO cannot access /usr/share

        var chunks = await transferenceService.GetTransferChunks(transference.IdTransference);
        if (chunks == null)
        {
            return;
        }

        if (chunks.Any((chunk) => chunk.EnChunkStatus != EnChunkStatus.Received))
        {
            return;
        }
        var allLocally = chunks.Select(CheckForChunkLocally).All(found => found);
        if (!allLocally)
        {
            return;
        }
        await AssembleFile(transference, chunks);
    }

    private async Task AssembleFile(Transference transference, List<TransferenceChunk> chunks)
    {
        var sortedChunks = chunks.OrderBy((chunk) => chunk.StartByteIndex);
        var outputFilePath = Path.Combine(GetTransferDirectory(), transference.DestinationPath);

        await using (var outputStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
        {
            foreach (var chunk in sortedChunks)
            {
                var chunkPath = GetChunkPath(chunk);
                await using var chunkStream = new FileStream(chunkPath, FileMode.Open, FileAccess.Read);
                await chunkStream.CopyToAsync(outputStream);
            }
        }

        foreach (var chunk in chunks)
        {
            File.Delete(GetChunkPath(chunk));
        }
    }
    private string GetChunkPath(TransferenceChunk chunk)
    {
        var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), chunk.IdTransference.ToString());
        return Path.Combine(directory, $"{chunk.StartByteIndex}.bin");
    }
    private bool CheckForChunkLocally(TransferenceChunk chunk)
    {
        var chunkPath = GetChunkPath(chunk);
        return File.Exists(chunkPath);
    }

    private string GetTransferDirectory()
    {
        var localStorageContent = new LocalStorage().GetStorage();
        return localStorageContent?.DefaultReceivingFolder ??
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "MobileLink");
    }
}