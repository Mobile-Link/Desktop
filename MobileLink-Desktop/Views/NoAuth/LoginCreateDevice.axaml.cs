using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using MobileLink_Desktop.ViewModels.NoAuth;

namespace MobileLink_Desktop.Views.NoAuth;

public partial class LoginCreateDevice : UserControl
{
    public LoginCreateDevice(string login, string password, string code)
    {
        var vm = App.AppServiceProvider.GetRequiredService<LoginCreateDeviceViewModel>();
        vm.code = code;
        vm.login = login;
        vm.password = password;
        DataContext = vm;

        InitializeComponent();
    }
}