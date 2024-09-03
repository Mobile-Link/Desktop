using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using MobileLink_Desktop.Interfaces;

namespace MobileLink_Desktop.Utils;

public class SocketMethods
{
    private readonly HubConnection _serverConnection;
    private readonly ServerconnectionStatus _serverStatus;

    private HubConnection ServerConnectionValidated
    {
        get
        {
            if (_serverStatus != ServerconnectionStatus.Connected)
            {
                throw new Exception("Connection not established");
            }

            return _serverConnection;
        }
    }
    
    public SocketMethods(
        ServerConnection con
    )
    {
        _serverStatus = con.Status;
        _serverConnection = con.Connection;
    }
    
    public Task SendMessage(string message)
    {
        Console.WriteLine("Sending message");
        return ServerConnectionValidated.SendAsync("SendMessage", "1", message);
    }
    
    public Task SendFile(byte[] chunk)
    {
        Console.WriteLine("Sending file");
        return ServerConnectionValidated.SendAsync(
            "SendFile",
            1,
            "testo",
            chunk
        );
    }

}
