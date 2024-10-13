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
    private Window? _mainWindow;
    public static IServiceProvider AppServiceProvider { get; private set; }
    private readonly NavigationService _navigationService;

    public App()
    {
        var collection = new ServiceCollection();
        collection.AddCommonServices();
        AppServiceProvider = collection.BuildServiceProvider();
        var socketConnection = AppServiceProvider.GetRequiredService<SocketConnection>();
        _navigationService = AppServiceProvider.GetRequiredService<NavigationService>();
        socketConnection.Connect();
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
            desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }
    }

    private void VerifyLogIn(bool openWindow) //change name
    {
        const bool loggedIn = true;
        const bool openWindowOnStartup = true; //TODO add this to localstorage 

        if (!loggedIn)
        {
            var vm = AppServiceProvider.GetRequiredService<LoginRegisterViewModel>();
            ChangeWindow(vm, new NoAuthLayout(), new LoginRegister());
            return;
        }

        if (openWindowOnStartup || openWindow)
        {
            var vm = AppServiceProvider.GetRequiredService<TransferenceViewModel>();
            ChangeWindow(vm, new AuthLayout(), new Transference());
        }
    }

    private void OpenWindow(object? sender, EventArgs eventArgs)
    {
        VerifyLogIn(true);
    }

    private void ChangeWindow(BaseViewModel viewModel, Window window, UserControl content)
    {
        if (_mainWindow != null)
        {
            _mainWindow.Close();
        }

        _mainWindow = window;
        _navigationService.Initialize(_mainWindow);
        _navigationService.NavigateToRoot(viewModel, content);
        _mainWindow.Show();
    }
}