using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Collections;
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
public class EmailValidationViewModel(Navigation navigation, AuthService authService) : BaseViewModel
{
    private AvaloniaList<char> _code = new AvaloniaList<char>() { '-', '-', '-', '-', '-', '-' };//TODO this avalonia list doesnt trigger properly, I think
    public string email = string.Empty;
    public string login = string.Empty;
    public string password = string.Empty;
    public SwitchNextScreen nextScreen = SwitchNextScreen.CreateAccount;

    public AvaloniaList<char> Code
    {
        get => _code;
        set
        {
            _code = value;
            NotifyPropertyChanged(nameof(Code)); 
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
        var formatedCode = string.Join("", _code.ToArray());
        authService.VerifyCode(email, formatedCode).ContinueWith((verifyTask) =>
        {
            if (!verifyTask.Result)
            {
                //TODO error
                return;
            }
            Dispatcher.UIThread.Post(() =>
            { 
                navigation.NavigateTo(new CreateAccount(email, formatedCode));
            }, DispatcherPriority.Background);
        });
    }
    private void GotoLoginCreateDevice()
    {
        var formatedCode = string.Join("", _code.ToArray());
        authService.VerifyCode(login, formatedCode).ContinueWith((verifyTask) =>
        {
            if (!verifyTask.Result)
            {
                //TODO error
                return;
            }
            Dispatcher.UIThread.Post(() =>
            { 
                navigation.NavigateTo(new LoginCreateDevice(login, password, formatedCode));
            }, DispatcherPriority.Background);
        });
    }
    public void GoBack()
    {
        navigation.NavigateToBack();
    }
}