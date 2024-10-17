using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MobileLink_Desktop.ViewModels.NoAuth;

namespace MobileLink_Desktop.Views.NoAuth;

public partial class Register : UserControl
{
    public Register()
    {
        DataContext = App.AppServiceProvider.GetRequiredService<RegisterViewModel>();
        InitializeComponent();
    }
}