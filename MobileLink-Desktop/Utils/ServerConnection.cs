using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace MobileLink_Desktop.Utils;

public class ServerConnection
{
    private HubConnection connection;

    private static ServerConnection? instance;

    private ServerConnection(HubConnection con)
    {
        connection = con;
        HubListener();
    }

    public static async Task<ServerConnection> GetInstance()
    {
        if (instance == null)
        {
            var con = await Connect();
            instance = new ServerConnection(con);
        }
        return instance;
    }
    private static async Task<HubConnection> Connect()
    {
        Console.WriteLine("Connecting...");
        HubConnection connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/chatHub")
            .Build();
        await connection.StartAsync();
        return connection;
    }

    private void HubListener()
    {
        connection.On<string, string>("ReceiveMessage", (userId, message) =>
        {
            Console.WriteLine($"Received {userId}: {message}");
        });
    }
    
    public Task SendMessage(string message)
    {
        Console.WriteLine("Sending message");
        // connection.
        return connection.SendAsync("SendMessage", "1", message);
    }
    public Task SendFile(byte[] chunk)
    {
        Console.WriteLine("Sending file");
        // connection.
        return connection.SendAsync("SendFile", new {
            userId= 1,
            chunk
        });
    }
    
}