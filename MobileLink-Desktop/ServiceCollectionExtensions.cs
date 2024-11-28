using Microsoft.Extensions.DependencyInjection;
using MobileLink_Desktop.Service;
using MobileLink_Desktop.Service.ApiServices;
using MobileLink_Desktop.Utils;
using MobileLink_Desktop.ViewModels.NoAuth;
using MobileLink_Desktop.ViewModels.Auth;
using MobileLink_Desktop.ViewModels.Dialog;
using MobileLink_Desktop.Views.NoAuth;

namespace MobileLink_Desktop;

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {
        collection.AddTransient<ServerAPI>();
        collection.AddTransient<DeviceService>();
        collection.AddTransient<ConnectionService>();
        collection.AddTransient<AuthService>();
        collection.AddTransient<TransferenceService>();
        
        collection.AddSingleton<Navigation>();
        collection.AddSingleton<TransferenceTimer>();
        collection.AddSingleton<SocketConnection>();
        collection.AddSingleton<TransferenceHandler>();
        collection.AddTransient<Session>();

        collection.AddTransient<LoginViewModel>();
        collection.AddTransient<RegisterViewModel>();
        collection.AddTransient<LoginRegisterViewModel>();
        collection.AddTransient<EmailValidationViewModel>();
        collection.AddTransient<TransferenceViewModel>();
        collection.AddTransient<CreateAccountViewModel>();
        collection.AddTransient<LoginCreateDevice>();
        collection.AddTransient<LoginCreateDeviceViewModel>();
        
        collection.AddTransient<SelectFolderDialogViewModel>();
        collection.AddTransient<SwalDialogViewModel>();
    }
}