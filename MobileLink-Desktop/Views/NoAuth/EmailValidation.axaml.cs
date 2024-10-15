using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MobileLink_Desktop.ViewModels.NoAuth;

namespace MobileLink_Desktop.Views.NoAuth;

public partial class EmailValidation : UserControl
{
    public EmailValidation(string email)
    {
        var vm = App.AppServiceProvider.GetRequiredService<EmailValidationViewModel>();
        vm.email = email;
        DataContext = vm;
        InitializeComponent();
    }
}