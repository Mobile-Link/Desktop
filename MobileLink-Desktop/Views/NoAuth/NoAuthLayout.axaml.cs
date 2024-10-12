using Avalonia.Controls;


namespace MobileLink_Desktop.Views.NoAuth;

public partial class NoAuthLayout : Window
{
    public NoAuthLayout()
    {
        InitializeComponent();
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        var screen = Screens.Primary;
        if (screen == null)
        {
            return;
        }
        Height = screen.Bounds.Height * 0.6;
        Width = screen.Bounds.Width * 0.6;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }
}