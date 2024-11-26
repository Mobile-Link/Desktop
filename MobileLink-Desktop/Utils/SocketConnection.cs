using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using MobileLink_Desktop.Interfaces;

namespace MobileLink_Desktop.Utils;

public class SocketConnection
{
    private TransferenceHandler _transferenceHandler;
    public HubConnection Connection { get; private set; }
    public EnServerconnectionStatusType StatusType = EnServerconnectionStatusType.Disconnected;

    public SocketConnection(TransferenceHandler transferenceHandler)
    {
        _transferenceHandler = transferenceHandler;
        var storageContent = new LocalStorage().GetStorage();
        Connection = new HubConnectionBuilder()
            .WithUrl($"http://localhost:5000/connectionhub", options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(storageContent?.Token);
            })
            .Build();
    }

    public async Task Connect()
    {
        if (Connection.State == HubConnectionState.Connected)
        {
            return;
        }
        var storageContent = new LocalStorage().GetStorage();
        if (storageContent?.IdDevice == null)
        {
            StatusType = EnServerconnectionStatusType.UnAuthorized;
            return;
        }
        Console.WriteLine("Im going to connect");
        Connection = new HubConnectionBuilder()
            .WithUrl($"http://localhost:5000/connectionhub", options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(storageContent?.Token);
            })
            .Build();
        await Connection.StartAsync();
        Console.WriteLine($"Connection: {Connection.ConnectionId}");
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
        
        Connection.On<int, long, byte[]>("ReceiveFileChunk", _transferenceHandler.ReceiveFileChunk);
        Connection.On<int>("ReSendChunks", _transferenceHandler.ReSendChunksHandler);
        Connection.On<int, string, long>("ReceiveNewTransference", ReceiveNewTransference);
    }

    private void ReceiveNewTransference(int idTransfer, string filePath, long fileSize)
    {
        Console.WriteLine($"New transfer started {idTransfer}, {filePath}, {fileSize}");
        //TODO check if this is needed
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
        //TODO redesign function to retry from time to time or a big sleep between each
        StatusType = EnServerconnectionStatusType.CantConnect;
    }
}