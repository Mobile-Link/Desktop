using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using MobileLink_Desktop.Utils;
using MobileLink_Desktop.ViewModels;
using Newtonsoft.Json;


// https://learn.microsoft.com/en-us/uwp/api/windows.networking.sockets.streamwebsocket?view=winrt-26100
namespace MobileLink_Desktop;

public partial class MainWindow : MainLayout
{
    private SocketMethods? _socketMethods;
    
    private SocketMethods SocketMethodsValidated
    {
        get
        {
            if (_socketMethods == null)
            {
                throw new Exception("Connection not established");
            }
            return _socketMethods;
        }
    }
    public MainWindow()
    {
        InitializeComponent();
        ServerConnection.GetInstance().ContinueWith((Task<ServerConnection> conTask) =>
        {
            if (conTask.Status != TaskStatus.RanToCompletion)
            {
                //TODO Throw error to user informing of no connection
                return;
            }
            _socketMethods = new SocketMethods(conTask.Result);
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
        SocketMethodsValidated.SendMessage(message);
    }
    
    private void SendFile(object sender, RoutedEventArgs e)
    {
        PickFile().ContinueWith((fileTask) =>
        {
            var file = fileTask.Result;
            var firstChunk = Encoding.UTF8.GetBytes(file.Split("\n")[0]);
            _socketMethods.SendFile(firstChunk);
        });
    }
    
    private async Task<string> PickFile()
    {
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Text File",
            AllowMultiple = false
        });

        if (files.Count >= 1)
        {
            // Open reading stream from the first file.
            await using var stream = await files[0].OpenReadAsync();
            using var streamReader = new StreamReader(stream);
            // Reads all the content of file as a text.
            var fileContent = await streamReader.ReadToEndAsync();
            return fileContent;
        }

        return "";
    }
}