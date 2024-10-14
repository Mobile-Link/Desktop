using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using MobileLink_Desktop.Interfaces;

namespace MobileLink_Desktop.Utils;

public class SocketMethods(SocketConnection con)
{
    private readonly HubConnection _serverConnection = con.Connection;
    private readonly EnServerconnectionStatusType _serverStatusType = con.StatusType;

    private HubConnection ServerConnectionValidated
    {
        get
        {
            if (_serverStatusType != EnServerconnectionStatusType.Connected)
            {
                throw new Exception("Connection not established");
            }

            return _serverConnection;
        }
    }

    public Task StartTransfer(long idDevice, string filePath, long fileSize, string destinationPath)
    {
        return ServerConnectionValidated.SendAsync("StartTransference", idDevice, filePath, fileSize, destinationPath);
    }
    public Task SendPacket(long idTransfer, long startByteIndex, byte[] byteArray)
    {
        return ServerConnectionValidated.SendAsync("SendFileChunk", idTransfer, startByteIndex, byteArray);
    }
}
