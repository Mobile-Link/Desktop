using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using MobileLink_Desktop.Interfaces;

namespace MobileLink_Desktop.Utils;

public class SocketConnection
{
    public HubConnection Connection { get; private set; } = new HubConnectionBuilder()
        .WithUrl("http://localhost:5000/transferhub")
        .Build();

    public EnServerconnectionStatusType StatusType = EnServerconnectionStatusType.Disconnected;


    public async Task Connect()
    {
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