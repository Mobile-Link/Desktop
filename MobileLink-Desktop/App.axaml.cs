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
    private readonly LocalStorage _localStorage;
    private SocketConnection _socketConnection;
    public App()
    {
        var collection = new ServiceCollection();
        collection.AddCommonServices();
        AppServiceProvider = collection.BuildServiceProvider();
        _socketConnection = AppServiceProvider.GetRequiredService<SocketConnection>();
        _navigationService = AppServiceProvider.GetRequiredService<NavigationService>();
        _localStorage = AppServiceProvider.GetRequiredService<LocalStorage>();

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
        var storageContent = _localStorage.GetStorage();
        if (storageContent == null || storageContent?.Token == null)
        {
            var vm = AppServiceProvider.GetRequiredService<LoginRegisterViewModel>();
            ChangeWindow(vm, new NoAuthLayout(), new LoginRegister());
            return;
        }
        _socketConnection.Connect();
        if (storageContent.OpenWindowOnStartUp || openWindow)
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
    public static Window? GetMainWindow()
    {
        if (Application.Current?.ApplicationLifetime is not ClassicDesktopStyleApplicationLifetime desktopLifetime)
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