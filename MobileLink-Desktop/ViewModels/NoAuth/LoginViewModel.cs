using System;
using System.ComponentModel;
using System.Threading.Tasks;
using MobileLink_Desktop.Utils;

namespace MobileLink_Desktop.ViewModels.NoAuth;

public class LoginViewModel(ServerAPI api) : BaseViewModel
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
        await api.Login(_emailUser, _password);
    }
}