using Microsoft.Extensions.DependencyInjection;
using MobileLink_Desktop.Service;
using MobileLink_Desktop.Service.ApiServices;
using MobileLink_Desktop.Utils;
using MobileLink_Desktop.ViewModels.NoAuth;
using MobileLink_Desktop.ViewModels.Auth;
using MobileLink_Desktop.Views.NoAuth;

namespace MobileLink_Desktop;

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {
        collection.AddSingleton<NavigationService>();
        
        collection.AddSingleton<SocketConnection>();
        collection.AddTransient<SocketMethods>();
        
        collection.AddTransient<ServerAPI>();
        collection.AddTransient<DeviceService>();
        collection.AddTransient<ConnectionService>();
        collection.AddTransient<AuthService>();
        collection.AddTransient<SessionService>();

        collection.AddTransient<LoginViewModel>();
        collection.AddTransient<RegisterViewModel>();
        collection.AddTransient<LoginRegisterViewModel>();
        collection.AddTransient<EmailValidationViewModel>();
        collection.AddTransient<TransferenceViewModel>();
        collection.AddTransient<CreateAccountViewModel>();
        collection.AddTransient<LoginCreateDevice>();
        collection.AddTransient<LoginCreateDeviceViewModel>();
    }
}