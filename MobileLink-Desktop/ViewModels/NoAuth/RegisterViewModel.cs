using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using MobileLink_Desktop.Service;
using MobileLink_Desktop.Service.ApiServices;
using MobileLink_Desktop.Utils;
using MobileLink_Desktop.Views.NoAuth;

namespace MobileLink_Desktop.ViewModels.NoAuth;

public class RegisterViewModel(AuthService authService, NavigationService navigationService) : BaseViewModel
{
    private string _email = string.Empty;

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            NotifyPropertyChanged(Email); 
        }
    }
    public void SubmitValidateEmail()
    {
        authService.SendCode(_email).ContinueWith((sendTask) =>
        {
            Dispatcher.UIThread.Post(() =>
            { 
                navigationService.NavigateTo(new EmailValidation(_email));
            }, DispatcherPriority.Background);
        });

    }
}