using System;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using MobileLink_Desktop.Utils;
using MobileLink_Desktop.ViewModels;
using Newtonsoft.Json;


// https://learn.microsoft.com/en-us/uwp/api/windows.networking.sockets.streamwebsocket?view=winrt-26100
namespace MobileLink_Desktop;

public partial class MainLayout : Window
{
    // private 
    public MainLayout()
    {
        InitializeComponent();
        AddHandler( DragDrop.DropEvent, OnDrop);
    }
    
    public new object? Content
    {
        get => MainContent.Content;
        set => MainContent.Content = value;
    }
    
    private static void OnDrop( object? sender, DragEventArgs e )
    {
        var items = e.Data.GetFiles(); 
        if ( items != null )
        {
            Trace.WriteLine( $"Dropped {items.FirstOrDefault()}" );
        }
    }
}