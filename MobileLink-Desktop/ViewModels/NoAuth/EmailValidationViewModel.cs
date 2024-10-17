using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Threading;
using MobileLink_Desktop.Service;
using MobileLink_Desktop.Service.ApiServices;
using MobileLink_Desktop.Views.NoAuth;

namespace MobileLink_Desktop.ViewModels.NoAuth;

public enum SwitchNextScreen
{
    CreateAccount,
    LoginCreateDevice
}
public class EmailValidationViewModel(NavigationService navigationService, AuthService authService) : BaseViewModel
{
    private string _code = string.Empty;
    public string email = string.Empty;
    public string login = string.Empty;
    public string password = string.Empty;
    public SwitchNextScreen nextScreen = SwitchNextScreen.CreateAccount;
    
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
        switch (nextScreen)
        {
            case SwitchNextScreen.CreateAccount:
                GotoCreateAccount();
                break;
            case SwitchNextScreen.LoginCreateDevice:
                GotoLoginCreateDevice();
                break;
        }

    }

    private void GotoCreateAccount()
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
    private void GotoLoginCreateDevice()
    {
        authService.VerifyCode(login, _code).ContinueWith((verifyTask) =>
        {
            if (!verifyTask.Result)
            {
                //TODO error
                return;
            }
            Dispatcher.UIThread.Post(() =>
            { 
                navigationService.NavigateTo(new LoginCreateDevice(login, password, _code));
            }, DispatcherPriority.Background);
        });
    }
    public void GoBack()
    {
        navigationService.NavigateToBack();
    }
}