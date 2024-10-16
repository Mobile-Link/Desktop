using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Threading;
using MobileLink_Desktop.Interfaces;
using MobileLink_Desktop.Utils;
using MobileLink_Desktop.Views.Auth;
using MobileLink_Desktop.Views.NoAuth;

namespace MobileLink_Desktop.Service;

public class SessionService(SocketConnection socketConnection, NavigationService navigationService)
{
    public void VerifyLogIn(bool openWindow) //change name
    {
        var storageContent = new LocalStorage().GetStorage();
        if (storageContent == null || storageContent?.Token == null)
        {
            Dispatcher.UIThread.Post(() => { navigationService.UpdateWindow(new NoAuthLayout(), new LoginRegister()); },
                DispatcherPriority.Background);
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
                Dispatcher.UIThread.Post(
                    () => { navigationService.UpdateWindow(new AuthLayout(), new Transference()); },
                    DispatcherPriority.Background);
            });
        }
    }
}