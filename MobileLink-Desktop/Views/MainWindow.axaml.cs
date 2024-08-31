using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MobileLink_Desktop.Utils;
using MobileLink_Desktop.ViewModels;
using Newtonsoft.Json;


// https://learn.microsoft.com/en-us/uwp/api/windows.networking.sockets.streamwebsocket?view=winrt-26100
namespace MobileLink_Desktop;

public partial class MainWindow : Window
{
    private ServerConnection _connection;
    // private 
    public MainWindow()
    {
        InitializeComponent();
        ServerConnection.GetInstance().ContinueWith((Task<ServerConnection> conTask) =>
        {
            _connection = conTask.Result;
        });
        DataContext = new MainWindowViewModel();
    }

    private MainWindowViewModel GetDataContext()
    {
        if (DataContext == null)
        {
            throw new Exception("Error on getting data context");
        }
        return (MainWindowViewModel)DataContext;
    }
    
    private void SendMessage(object? sender, RoutedEventArgs e)
    {
        var message = GetDataContext().Message;
        _connection.SendMessage(message);
    }
}