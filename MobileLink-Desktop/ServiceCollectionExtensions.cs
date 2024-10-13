using Microsoft.Extensions.DependencyInjection;
using MobileLink_Desktop.Service;
using MobileLink_Desktop.Utils;
using MobileLink_Desktop.ViewModels.NoAuth;

namespace MobileLink_Desktop;

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {
        collection.AddSingleton<SocketConnection>();
        collection.AddSingleton<NavigationService>();
        collection.AddScoped<ServerAPI>();

        collection.AddTransient<LoginViewModel>();
        collection.AddTransient<RegisterViewModel>();
        collection.AddTransient<LoginRegisterViewModel>();
        collection.AddTransient<EmailValidationViewModel>();
    }
}