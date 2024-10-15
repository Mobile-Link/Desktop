using MobileLink_Desktop.Classes.Http.Request;
using MobileLink_Desktop.Service;
using MobileLink_Desktop.Service.ApiServices;

namespace MobileLink_Desktop.ViewModels.NoAuth;

public class CreateAccountViewModel(NavigationService navigationService, AuthService authService) : BaseViewModel
{
    public string code = string.Empty;
    public string email = string.Empty;
    private string _username = string.Empty;
    private string _password = string.Empty;
    private string _confirmPassword = string.Empty;
    private string _deviceName = string.Empty;
    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            NotifyPropertyChanged(Username); 
        }
    }
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            NotifyPropertyChanged(Password); 
        }
    }
    public string ConfirmPassword
    {
        get => _confirmPassword;
        set
        {
            _confirmPassword = value;
            NotifyPropertyChanged(ConfirmPassword); 
        }
    }
    public string DeviceName
    {
        get => _deviceName;
        set
        {
            _deviceName = value;
            NotifyPropertyChanged(DeviceName); 
        }
    }

    public void SubmitRegister()
    {
        authService.Register(new RegisterRequest()
        {
            Email = email,
            Username = _username,
            Password = _password,
            Code = code,
            DeviceName = _deviceName
        }).ContinueWith((taskRegister) =>
        {
            //TODO get failure or token, when token put it on storage and go to homepage 
        });
    }
    //TODO back button
}