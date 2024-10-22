using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Threading;
using MobileLink_Desktop.Classes;
using MobileLink_Desktop.Interfaces;
using MobileLink_Desktop.Service.ApiServices;
using MobileLink_Desktop.Utils;
using MobileLink_Desktop.Views.Auth;
using MobileLink_Desktop.Views.NoAuth;

namespace MobileLink_Desktop.Service;

public class SessionService(SocketConnection socketConnection, NavigationService navigationService, AuthService authService)
{
    public async void VerifyLogIn(bool openWindow) //change name
    {
        var storageContent = new LocalStorage().GetStorage();
        if (storageContent == null || storageContent?.Token == null)
        {
            ShowInitialLayout(false);
            return;
        }

        var authorized = await authService.VerifyToken();
        if (!authorized)
        {
            ShowInitialLayout(false);
            return;
        }
        if (storageContent.OpenWindowOnStartUp || openWindow)
        {
            var tasks = new List<Task>();
            if (socketConnection.StatusType != EnServerconnectionStatusType.Connected)
            {
                tasks.Add(socketConnection.Connect());
            }

            Task.WhenAll(tasks.ToArray()).ContinueWith((_) =>
            {
                ShowInitialLayout(true);
            });
        }
    }

    public void UpdateTokenAndAuthorize(string token, int idDevice)
    {
        var localStorage = new LocalStorage();
        var localStorageContent = localStorage.GetStorage();
        localStorageContent ??= new LocalStorageContent();
        localStorageContent.Token = token;
        localStorageContent.IdDevice = idDevice;
        localStorage.SetStorage(localStorageContent);
        VerifyLogIn(true);
    }

    private void ShowInitialLayout(bool authorized)
    {
        if (!authorized)
        {
            Dispatcher.UIThread.Post(() => { navigationService.UpdateWindow(new NoAuthLayout(), new LoginRegister()); },
                DispatcherPriority.Background);
            return;
        }
        Dispatcher.UIThread.Post(
            () => { navigationService.UpdateWindow(new AuthLayout(), new Transference()); },
            DispatcherPriority.Background);
    }
}