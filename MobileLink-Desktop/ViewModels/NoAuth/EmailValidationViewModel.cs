using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Threading;
using MobileLink_Desktop.Service;
using MobileLink_Desktop.Service.ApiServices;
using MobileLink_Desktop.Views.NoAuth;

namespace MobileLink_Desktop.ViewModels.NoAuth;

public class EmailValidationViewModel(NavigationService navigationService, AuthService authService) : BaseViewModel
{
    private string _code = string.Empty;
    public string email = string.Empty;
    
    public string Code
    {
        get => _code;
        set
        {
            _code = value;
            NotifyPropertyChanged(Code); 
        }
    }

    public void SubmitVerificationCode()
    {
        authService.VerifyCode(email, _code).ContinueWith((verifyTask) =>
        {
            if (!verifyTask.Result)
            {
                //TODO error
                return;
            }
            Dispatcher.UIThread.Post(() =>
            { 
                navigationService.NavigateTo(new CreateAccount(email, _code));
            }, DispatcherPriority.Background);
        });
    }
    public void GoBack()
    {
        navigationService.NavigateToBack();
    }
}