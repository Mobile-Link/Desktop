using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using MobileLink_Desktop.Interfaces;

namespace MobileLink_Desktop.Utils;

public class SocketMethods(SocketConnection con)
{
    public Task SendPacket(int idTransfer, long startByteIndex, byte[] byteArray)
    {
        if (con.StatusType != EnServerconnectionStatusType.Connected)
        {
            throw new Exception("Connection not established");
        }
        
        return con.Connection.SendAsync("SendFileChunk", idTransfer, startByteIndex, byteArray);
    }
}
