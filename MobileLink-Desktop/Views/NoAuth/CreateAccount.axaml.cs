using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MobileLink_Desktop.ViewModels.NoAuth;

namespace MobileLink_Desktop.Views.NoAuth;

public partial class CreateAccount : UserControl
{
    public CreateAccount(string email, string code)
    {
        var vm = App.AppServiceProvider.GetRequiredService<CreateAccountViewModel>();
        vm.email = email;
        vm.code = code;
        DataContext = vm;
        InitializeComponent();
    }
}