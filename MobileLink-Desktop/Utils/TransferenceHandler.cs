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

        var chunkPath = GetChunkPathAndCreateDirectory(new TransferenceChunk()
            { StartByteIndex = startByteIndex, IdTransference = idTransfer });

        if (File.Exists(chunkPath))
        {
            return;
        }
        
        await File.WriteAllBytesAsync(chunkPath, byteArray);

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
            //TODO add timer
            return;
        }
        //TODO this is being called multiple times for the same transfer
        await AssembleFile(transference, chunks);
    }

    public async Task CheckTransfersNotReceived()
    {
        var notReceived = await transferenceService.GetTransfersNotOnDestination();
        if (notReceived.Count == 0)
        {
            return;
        }

        foreach (var transfer in notReceived)
        {
            GetChunksNotLocally(transfer).ContinueWith(_ => { });
        }
    }

    public async Task CheckTransfersNotSent()
    {
        var notSent = await transferenceService.GetTransfersNotOnServer();
        if (notSent.Count == 0)
        {
            return;
        }

        foreach (var transfer in notSent)
        {
            var chunks = await transferenceService.GetTransferChunks(transfer.IdTransference);
            if (chunks == null)
            {
                continue;
            }
            foreach (var chunk in chunks.Where(chunk => chunk.EnChunkStatus == EnChunkStatus.Pending))
            {
                ReSendChunk(transfer, chunk)
                    .ContinueWith(_ => {});
            }
        }
    }

    public async Task ReSendChunkHandler(int idChunk)
    {
        var chunk = await transferenceService.GetChunkWithTransference(idChunk);
        if (chunk?.Transference == null)
        {
            return;
        }

        var transfer = chunk.Transference;
        await ReSendChunk(transfer, chunk);
    }

    private async Task GetChunksNotLocally(Transference transference)
    {
        var chunks = await transferenceService.GetTransferChunks(transference.IdTransference);
        if (chunks == null)
        {
            return;
        }

        var chunksNotLocally = chunks.Where(chunk => !CheckForChunkLocally(chunk)).ToList();
        if (chunksNotLocally.Count == 0)
        {
            AssembleFile(transference, chunks).ContinueWith(_ => {});
        }
        foreach (var chunk in chunksNotLocally)
        {
            transferenceService.GetChunkBytes(chunk.IdTransferenceChunk).ContinueWith((resBytes) =>
            {
                var response = resBytes.Result;
                if (response == null)
                {
                    return;
                }

                var receivedChunk = response.Chunk;
                ReceiveFileChunk(receivedChunk.IdTransference, receivedChunk.StartByteIndex, response.Bytes)
                    .ContinueWith(_ => { });
            });
        }
    }

    //TODO call this when on startup in case of finished transfer
    private async Task AssembleFile(Transference transference, List<TransferenceChunk> chunks)
    {
        var sortedChunks = chunks.OrderBy((chunk) => chunk.StartByteIndex);
        var directory = transference.DestinationPath == "/" ? 
            GetTransferDirectory()
            : Path.Combine(GetTransferDirectory(), transference.DestinationPath);
        var outputFilePath = Path.Combine(directory, transference.FileNameExtension);
        Directory.CreateDirectory(directory);
        await using (var outputStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
        {
            foreach (var chunk in sortedChunks)
            {
                var chunkPath = GetChunkPathAndCreateDirectory(chunk);
                await using var chunkStream = new FileStream(chunkPath, FileMode.Open, FileAccess.Read);
                await chunkStream.CopyToAsync(outputStream);
            }
        }
        //TODO call api to cleanup and update to completed
        foreach (var chunk in chunks)
        {
            File.Delete(GetChunkPathAndCreateDirectory(chunk));
        }
        var chunksDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MobileLink",
            transference.IdTransference.ToString());
        Directory.Delete(chunksDirectory); //TODO test this
    }

    private string GetChunkPathAndCreateDirectory(TransferenceChunk chunk)
    {
        var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MobileLink",
            chunk.IdTransference.ToString());
        Directory.CreateDirectory(directory);
        return Path.Combine(directory, $"{chunk.StartByteIndex}.bin");
    }

    private bool CheckForChunkLocally(TransferenceChunk chunk)
    {
        var chunkPath = GetChunkPathAndCreateDirectory(chunk);
        return File.Exists(chunkPath);
    }

    private string GetTransferDirectory()
    {
        var localStorageContent = new LocalStorage().GetStorage();
        return localStorageContent?.DefaultReceivingFolder ??
               Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "MobileLink");
    }

    private async Task ReSendChunk(Transference transfer, TransferenceChunk chunk)
    {
        await using var fs = File.OpenRead(transfer.FilePath);
        const int chunkSize = 1024 * 1024;
        var byteArray = new byte[chunkSize];
        fs.Read(byteArray, (int)chunk.StartByteIndex, chunkSize);
        transferenceService.SendFileChunk(transfer.IdTransference, chunk.StartByteIndex, byteArray)
            .ContinueWith((_) => { });
    }

    public void TransferFile(int idDeviceDestination, string originalFilePath, string destinationPath)
    {
        var fileInfo = new FileInfo(originalFilePath);
        var fileLength = fileInfo.Length;
        var fileNameExtension = fileInfo.Name;
        var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MobileLink",
            "OutGoing", Guid.NewGuid().ToString());
        var copyPath = Path.Combine(directory, fileNameExtension);
        Directory.CreateDirectory(directory);
        try
        {
            File.Copy(originalFilePath, copyPath);
        }
        catch
        {
            //TODO throw error
            return;
        }

        transferenceService.StartTransference(idDeviceDestination, copyPath, fileNameExtension, fileLength, destinationPath)
            .ContinueWith(
                (taskStart) =>
                {
                    if (taskStart.Result == null)
                    {
                        //TODO error
                        return;
                    }

                    using (var fs = File.OpenRead(originalFilePath))
                    {
                        const int chunkSize = 1024 * 1024;

                        long startByteIndex = 0;

                        while (startByteIndex < fs.Length)
                        {
                            var byteArray = new byte[chunkSize];
                            fs.Read(byteArray, 0, chunkSize);
                            transferenceService.SendFileChunk(taskStart.Result ?? 0, startByteIndex, byteArray)
                                .ContinueWith(
                                    (_) => { });
                            startByteIndex += chunkSize;
                        }
                    }
                }
            );
    }
}