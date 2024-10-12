using System;
using System.ComponentModel;
using System.Threading.Tasks;
using MobileLink_Desktop.Utils;

namespace MobileLink_Desktop.ViewModels.NoAuth;

public class LoginViewModel : INotifyPropertyChanged
{
    private ServerAPI _api;

    public LoginViewModel(ServerAPI api)
    {
        _api = api;
    }

    private string _emailUser = string.Empty;

    public string EmailUser
    {
        get => _emailUser;
        set
        {
            _emailUser = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(EmailUser)); 
        }
    }
    private string _password = string.Empty;

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Password)); 
        }
    }

    public async Task SubmitLogin()
    {
        await _api.Login(_emailUser, _password);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}