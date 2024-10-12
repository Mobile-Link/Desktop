using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Threading;
using MobileLink_Desktop.Service;
using MobileLink_Desktop.Utils;
using MobileLink_Desktop.Views.NoAuth;

namespace MobileLink_Desktop.ViewModels.NoAuth;

public class RegisterViewModel(ServerAPI api, NavigationService navigationService) : INotifyPropertyChanged
{
    private string _email = string.Empty;

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Email)); 
        }
    }
    public async Task SubmitValidateEmail()
    {
        api.SendCode(_email).ContinueWith((sendTask) =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                navigationService.NavigateTo<EmailValidationViewModel>(new EmailValidation());
            }, DispatcherPriority.Background);
        });

    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
}