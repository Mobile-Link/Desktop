using System;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MobileLink_Desktop.Utils;

namespace MobileLink_Desktop;

public partial class App : Application
{
    private MainWindow? _mainWindow;
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }
        _ = ServerConnection.GetInstance().ContinueWith((_) =>
        {
            //debug purposes
            TrayIcon_OnClicked(new object(), EventArgs.Empty);
        });
    }
    
    private void TrayIcon_OnClicked(object? sender, EventArgs e)
    {
        _mainWindow = new MainWindow();
        _mainWindow.Show();
    }
}