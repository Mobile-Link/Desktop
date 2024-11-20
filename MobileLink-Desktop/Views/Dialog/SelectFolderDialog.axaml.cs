using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MobileLink_Desktop.ViewModels.Dialog;

namespace MobileLink_Desktop.Views.Dialog;

public partial class SelectFolderDialog : UserControl
{
    public SelectFolderDialog()
    {
        InitializeComponent();
    }

    private void CloseWindow(object? o, string? result)
    {
        ((Window)Parent!)?.Close(result);
    }
}