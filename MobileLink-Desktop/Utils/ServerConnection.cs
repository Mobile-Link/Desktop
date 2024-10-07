using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using MobileLink_Desktop.Interfaces;

namespace MobileLink_Desktop.Utils;

public class ServerConnection
{
    public HubConnection Connection { get; private set; }
    public EnServerconnectionStatusType StatusType = EnServerconnectionStatusType.Disconnected;

    private static ServerConnection? _instance;


    private ServerConnection(HubConnection con)
    {
        Connection = con;
        HubListener();
    }

    public static async Task<ServerConnection> GetInstance()
    {
        if (_instance != null)
        {
            return _instance;
        }
        try
        {
            var con = await Connect();
            _instance = new ServerConnection(con)
            {
                StatusType = EnServerconnectionStatusType.Connected
            };
            return _instance;
        }
        catch
        {
            throw new Exception("Error while trying to connect");
        }
    }

    private static async Task<HubConnection> Connect()
    {
        var connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/filetransferhub")
            .Build();
        await connection.StartAsync();
        return connection;
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
        var instance = await GetInstance();
        var retries = 3;
        while (retries > 0)
        {
            try
            {
                var con = await Connect();
                instance.Connection = con;
                instance.StatusType = EnServerconnectionStatusType.Connected;
                HubListener();
                break;
            }
            catch
            {
                retries--;
            }
        }

        instance.StatusType = EnServerconnectionStatusType.CantConnect;
    }
}