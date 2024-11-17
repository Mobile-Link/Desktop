using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MobileLink_Desktop.ViewModels.Dialog;

namespace MobileLink_Desktop.Views.Dialog;

public partial class SwalDialog : UserControl
{
    public SwalDialog(string confirmText, string title, string? bodyText, string? cancelText = null, ContentControl? bodyContent = null)
    {
        var vm = App.AppServiceProvider.GetRequiredService<SwalDialogViewModel>();
        vm.Title = title;
        vm.BodyText = bodyText;
        vm.ConfirmText = confirmText;
        vm.CancelText = cancelText;
        vm.BodyText = bodyText;
        vm.Result += CloseWindow;
        DataContext = vm;
        InitializeComponent();
    }

    private void CloseWindow(object? o, bool result)
    {
        ((Window)Parent!)?.Close(result);
    }
}