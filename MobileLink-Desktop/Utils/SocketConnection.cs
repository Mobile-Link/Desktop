using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using MobileLink_Desktop.Interfaces;

namespace MobileLink_Desktop.Utils;

public class SocketConnection
{
    public HubConnection Connection { get; private set; }
    public EnServerconnectionStatusType StatusType = EnServerconnectionStatusType.Disconnected;

    public SocketConnection()
    {
        var storageContent = new LocalStorage().GetStorage();
        Connection = new HubConnectionBuilder()
            .WithUrl($"http://localhost:5000/connectionhub?deviceId={storageContent?.IdDevice ?? 0}", options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(storageContent?.Token);
            })//TODO if no storage dont start connection
            .Build();
    }

    public async Task Connect()
    {
        var storageContent = new LocalStorage().GetStorage();
        if (storageContent?.IdDevice == null)
        {
            StatusType = EnServerconnectionStatusType.UnAuthorized;
            return;
        }
        Console.WriteLine("Im going to connect");
        await Connection.StartAsync();
        StatusType = EnServerconnectionStatusType.Connected;
        HubListener();
    }

    private void HubListener()
    {
        Connection.Closed += async (_) =>
        {
            StatusType = EnServerconnectionStatusType.Connecting;
            await RetryConnection();
            //TODO check if this works
        };
        Connection.On<string, string>("ReceiveMessage",
            (userId, message) => { Console.WriteLine($"Received {userId}: {message}"); });
        Connection.On<string,string>("UserConnected", 
            (userId, message) => { Console.WriteLine($"User {userId} : {message}");
            });

        Connection.On<string,string>("UserDisconnected", 
            (userId, message) => { Console.WriteLine($"User {userId} : {message}"); });
        
        Connection.On<long ,long, long, long, byte[]>("ReceiveFileChunk", ReceiveFileChunk);
        
        Connection.On<long>("FinalizeTransference", (idTransference =>
        {
            //TODO gather all chunks into a file 
        }));
    }

    private void ReceiveFileChunk(long idTransfer, long fileSize, long chunkSize, long startByteIndex, byte[] byteArray)
    {
        //TODO write
    }

    private async Task RetryConnection()
    {
        var retries = 3;
        while (retries > 0)
        {
            try
            {
                await Connect();
                StatusType = EnServerconnectionStatusType.Connected;
                HubListener();
                break;
            }
            catch
            {
                retries--;
            }
        }

        StatusType = EnServerconnectionStatusType.CantConnect;
    }
}