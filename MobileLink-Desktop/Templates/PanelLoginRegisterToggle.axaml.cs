using System.ComponentModel.Design;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;

namespace MobileLink_Desktop.Templates;

public class PanelLoginRegisterToggle : TemplatedControl
{
    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<PanelLoginRegisterToggle, string>(
            "Title", "Bem-Vindo de Volta", false, BindingMode.TwoWay);    
    public static readonly StyledProperty<string> ButtonTextProperty =
        AvaloniaProperty.Register<PanelLoginRegisterToggle, string>(
            "ButtonText", "Criar conta", false, BindingMode.TwoWay);    
    public static readonly StyledProperty<string> DescriptionProperty =
        AvaloniaProperty.Register<PanelLoginRegisterToggle, string>(
            "Description", "Ainda não tem conta?", false, BindingMode.TwoWay);
    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<PanelLoginRegisterToggle, ICommand>(
            "CommandProperty", null, false, BindingMode.TwoWay);

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    public string Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }
    public string ButtonText
    {
        get => GetValue(ButtonTextProperty);
        set => SetValue(ButtonTextProperty, value);
    }
    public ICommand Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
    // TODO move this to viewModel
}