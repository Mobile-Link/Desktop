using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MobileLink_Desktop.ViewModels.Auth;

namespace MobileLink_Desktop.Views.Auth;

public partial class Transference : UserControl
{
    public Transference()
    {
        DataContext = App.AppServiceProvider.GetRequiredService<TransferenceViewModel>();
        InitializeComponent();
    }
}