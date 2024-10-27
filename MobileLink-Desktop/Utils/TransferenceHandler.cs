using System;
using MobileLink_Desktop.Classes;
using MobileLink_Desktop.Service.ApiServices;

namespace MobileLink_Desktop.Utils;

public class TransferenceHandler(TransferenceService transferenceService)
{
    public void ReceiveFileChunk(int idTransfer, long startByteIndex, byte[] byteArray)
    {
        Console.WriteLine($"New chunk received {idTransfer}, {startByteIndex}, Length: {byteArray.Length}");
        var localStorage = new LocalStorage();
        var localStorageContent = localStorage.GetStorage();
        if (localStorageContent == null)
        {
            return;
        }
        var transference = transferenceService.GetTransfer(idTransfer);
        if (transference == null)
        {
            //Call api receiving error to retry
            return;
        }

        //TODO write chunk to SpecialFolder.CommonApplicationData
        
        //TODO check if all chunks have been received then write the file with the chunks
        
        //TODO if the file is correct call api and remove file from cloud
        
    }
}