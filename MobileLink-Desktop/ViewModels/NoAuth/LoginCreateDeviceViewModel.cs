using System;
using MobileLink_Desktop.Service;
using MobileLink_Desktop.Service.ApiServices;
using MobileLink_Desktop.Utils;

namespace MobileLink_Desktop.ViewModels.NoAuth;

public class LoginCreateDeviceViewModel(AuthService authService, Navigation navigation, Session session) : BaseViewModel
{
    private string _deviceName = string.Empty;
    public string login = string.Empty;
    public string password = string.Empty;
    public string code = string.Empty;

    public string DeviceName
    {
        get => _deviceName;
        set
        {
            _deviceName = value;
            NotifyPropertyChanged(DeviceName);
        }
    }

    public void CreateDevice()
    {
        authService.LoginCreateDevice(login, password, DeviceName, code).ContinueWith(async (taskLogin) =>
        {
            var result = taskLogin.Result;
            if (taskLogin.Result == null || taskLogin.Result.token == null)
            {
                //TODO error
                return;
            }
            await session.UpdateTokenAndAuthorize(result.token, result.idDevice);

        });
    }

    public void GoBack()
    {
        navigation.NavigateToBack();
    }
}