using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MobileLink_Desktop.ViewModels.NoAuth;

namespace MobileLink_Desktop.Views.NoAuth;

public partial class LoginRegister : UserControl
{
    public LoginRegister()
    {
        DataContext = App.AppServiceProvider.GetRequiredService<LoginRegisterViewModel>();
        InitializeComponent();
    }
}