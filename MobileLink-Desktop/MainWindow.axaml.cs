using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Newtonsoft.Json;


// https://learn.microsoft.com/en-us/uwp/api/windows.networking.sockets.streamwebsocket?view=winrt-26100
namespace MobileLink_Desktop;

public partial class MainWindow : Window
{
    private ClientWebSocket ws; 
    public MainWindow()
    {
        ws = new ClientWebSocket();
        InitializeComponent();
    }

    // https://gist.github.com/anonymous/574133a15f7faf39fdb5
    private void Call_WS(object? sender, RoutedEventArgs e)
    {
        ws.ConnectAsync(new Uri("ws://localhost:3000"), CancellationToken.None).ContinueWith((som) =>//doesnt seem to connect
        {
            return;
        });
    }
}