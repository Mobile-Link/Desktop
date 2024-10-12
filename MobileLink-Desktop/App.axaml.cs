using System;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using MobileLink_Desktop.Utils;
using MobileLink_Desktop.ViewModels.NoAuth;
using MobileLink_Desktop.Views.Auth;
using MobileLink_Desktop.Views.NoAuth;

namespace MobileLink_Desktop;

public partial class App : Application
{
    private Window? _mainWindow;
    public static IServiceProvider AppServiceProvider { get; private set; }
    private SocketConnection _socketConnection;

    public override void OnFrameworkInitializationCompleted()
    {
        var collection = new ServiceCollection();
        collection.AddCommonServices();
        AppServiceProvider = collection.BuildServiceProvider();
        _socketConnection = AppServiceProvider.GetRequiredService<SocketConnection>();
        _socketConnection.Connect();
        VerifyLogIn(false);
        base.OnFrameworkInitializationCompleted();
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }
        
    }
    
    private void VerifyLogIn(bool openWindow)//change name
    {
        const bool loggedIn = false;
        const bool openWindowOnStartup = false; //TODO add this to localstorage 
        
        if (!loggedIn)
        {
            var vm = AppServiceProvider.GetRequiredService<LoginRegisterViewModel>();
            ChangeWindow(new LoginRegister
            {
                DataContext = vm
            });
            return;
        }
        if (openWindowOnStartup || openWindow)
        {
            ChangeWindow(new AuthTest{});
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