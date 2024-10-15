using Microsoft.Extensions.DependencyInjection;
using MobileLink_Desktop.Service;
using MobileLink_Desktop.Service.ApiServices;
using MobileLink_Desktop.Utils;
using MobileLink_Desktop.ViewModels.NoAuth;
using MobileLink_Desktop.ViewModels.Auth;

namespace MobileLink_Desktop;

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {
        collection.AddSingleton<NavigationService>();
        collection.AddSingleton<LocalStorage>();
        
        collection.AddSingleton<SocketConnection>();
        collection.AddTransient<SocketMethods>();
        collection.AddScoped<ServerAPI>();
        collection.AddSingleton<DeviceService>();
        collection.AddSingleton<ConnectionService>();

        collection.AddTransient<LoginViewModel>();
        collection.AddTransient<RegisterViewModel>();
        collection.AddTransient<LoginRegisterViewModel>();
        collection.AddTransient<EmailValidationViewModel>();
        collection.AddTransient<TransferenceViewModel>();
    }
}