using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MobileLink_Desktop.ViewModels.Dialog;

namespace MobileLink_Desktop.Views.Dialog;

public partial class SwalDialog : UserControl
{
    public SwalDialog()
    {
        InitializeComponent();
    }

    private void CloseWindow(object? o, bool result)
    {
        ((Window)Parent!)?.Close(result);
    }
}