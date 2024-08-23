using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;


// https://learn.microsoft.com/en-us/uwp/api/windows.networking.sockets.streamwebsocket?view=winrt-26100
namespace MobileLink_Desktop;

public partial class MainWindow : Window
{
    
    private HubConnection _connection; 
    public MainWindow()
    {
        InitializeComponent();
        InitiateConnection();
    }

    private void InitiateConnection()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/chatHub")
            .Build();
        _connection.StartAsync().Wait();

        _connection.On<string, string>("ReceiveMessage", (user, message) =>
        {
            MessageListBox.Items.Add($"{user}: {message}");
        });
    }

    // https://gist.github.com/anonymous/574133a15f7faf39fdb5
    private void SendButtonClick(object? sender, RoutedEventArgs e)
    {
        _connection.SendAsync("SendMessage", "Client", MessageTextBox.Text).ContinueWith((asd) =>
        {
            MessageTextBox.Text = "";
        });
    }
}