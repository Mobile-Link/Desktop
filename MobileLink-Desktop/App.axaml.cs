using System;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using MobileLink_Desktop.Utils;
using MobileLink_Desktop.ViewModels.NoAuth;
using MobileLink_Desktop.Views.Auth;
using MobileLink_Desktop.Views.NoAuth;

namespace MobileLink_Desktop;

public partial class App : Application
{
    private Window? _mainWindow;
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }
        _ = ServerConnection.GetInstance().ContinueWith((_) =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                VerifyLogIn(false);
            }, DispatcherPriority.Background);
        });
    }
    
    private void VerifyLogIn(bool openWindow)//change name
    {
        var loggedIn = false;
        var openWindowOnStartup = false;//TODO add this to localstorage 
        
        if (!loggedIn)
        {
            ChangeWindow(new LoginRegister()
            {
                DataContext = new LoginRegisterViewModel()
            });
            return;
        }

        if (openWindowOnStartup || openWindow)
        {
            ChangeWindow(new AuthTest());
        }
    }

    private void OpenWindow(object? sender, EventArgs eventArgs)
    {
        VerifyLogIn(true);
    }

    private void ChangeWindow(Window window)
    {
        if (_mainWindow != null)
        {
            _mainWindow.Close();
        }
        _mainWindow = window;
        _mainWindow.Show();
    }
}