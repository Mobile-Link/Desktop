using System;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using MobileLink_Desktop.Service;
using MobileLink_Desktop.Utils;
using MobileLink_Desktop.ViewModels;
using MobileLink_Desktop.ViewModels.Auth;
using MobileLink_Desktop.ViewModels.NoAuth;
using MobileLink_Desktop.Views.Auth;
using MobileLink_Desktop.Views.NoAuth;

namespace MobileLink_Desktop;

public partial class App : Application
{
    public static IServiceProvider AppServiceProvider { get; private set; }
    private readonly NavigationService _navigationService;
    private readonly SocketConnection _socketConnection;
    public App()
    {
        var collection = new ServiceCollection();
        collection.AddCommonServices();
        AppServiceProvider = collection.BuildServiceProvider();
        _socketConnection = AppServiceProvider.GetRequiredService<SocketConnection>();
        _navigationService = AppServiceProvider.GetRequiredService<NavigationService>();

    }
    public override void OnFrameworkInitializationCompleted()
    {
        VerifyLogIn(false);
        base.OnFrameworkInitializationCompleted();
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            //TODO get from config file
            desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }
    }

    private void VerifyLogIn(bool openWindow) //change name
    {
        var session = AppServiceProvider.GetRequiredService<SessionService>();
        session.VerifyLogIn(openWindow);
    }

    private void OpenWindow(object? sender, EventArgs eventArgs)
    {
        VerifyLogIn(true);
    }
    
    public static void ChangeWindow(Window window)
    {
        if (Current?.ApplicationLifetime is not ClassicDesktopStyleApplicationLifetime desktopLifetime)
        {
            //TODO other lifetimes
            return;
        }
        if (desktopLifetime.MainWindow != null)
        {
            desktopLifetime.MainWindow.Close();
        }

        desktopLifetime.MainWindow = window;
        desktopLifetime.MainWindow.Show();
    }
    
    public static Window? GetMainWindow()
    {
        if (Current?.ApplicationLifetime is not ClassicDesktopStyleApplicationLifetime desktopLifetime)
        {
            return null;
        }
        if (desktopLifetime.MainWindow != null)
        {
            return desktopLifetime.MainWindow;
        }

        if (desktopLifetime.Windows.Count > 0)
        {
            return desktopLifetime.Windows[0];
        }

        return null;
    }
    
}