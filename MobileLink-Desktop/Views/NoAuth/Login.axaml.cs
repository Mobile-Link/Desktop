using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MobileLink_Desktop.ViewModels.NoAuth;

namespace MobileLink_Desktop.Views.NoAuth;

public partial class Login : UserControl
{
    public Login()
    {
        DataContext = App.AppServiceProvider.GetRequiredService<LoginViewModel>();
        InitializeComponent();
    }
}