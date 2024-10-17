using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Threading;
using MobileLink_Desktop.Service;
using MobileLink_Desktop.Service.ApiServices;
using MobileLink_Desktop.Utils;
using MobileLink_Desktop.Views.NoAuth;

namespace MobileLink_Desktop.ViewModels.NoAuth;

public class LoginViewModel(AuthService authService, NavigationService navigationService, SessionService sessionService) : BaseViewModel
{
    private string _emailUser = string.Empty;

    public string EmailUser
    {
        get => _emailUser;
        set
        {
            _emailUser = value;
            NotifyPropertyChanged(EmailUser); 
        }
    }
    private string _password = string.Empty;

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            NotifyPropertyChanged(Password); 
        }
    }

    public async Task SubmitLogin()
    {
        var storageContent = new LocalStorage().GetStorage();
        if (storageContent == null || storageContent?.IdDevice == null)
        {
            await authService.ValidateCredentials(_emailUser, _password).ContinueWith((taskVerify) =>
            {
                if (!taskVerify.Result.IsSuccessStatusCode)
                {
                    //TODO error
                    return;
                }
                Dispatcher.UIThread.Post(() =>
                { 
                    navigationService.NavigateTo(new EmailValidation(_emailUser, _password));
                }, DispatcherPriority.Background);
            });
            
            return;
        }

        await authService.Login(_emailUser, _password, storageContent.IdDevice ?? 0).ContinueWith((taskLogin) =>
        {
            var result = taskLogin.Result;
            if (result == null || result.token == null || result.idDevice == null)
            {
                //TODO error
                return;
            }
            sessionService.UpdateTokenAndAuthorize(result.token, result.idDevice);
        });
    }
}