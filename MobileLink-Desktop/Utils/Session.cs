using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Microsoft.VisualBasic.FileIO;
using MobileLink_Desktop.Classes;
using MobileLink_Desktop.Interfaces;
using MobileLink_Desktop.Service.ApiServices;
using MobileLink_Desktop.Utils;
using MobileLink_Desktop.Views.Auth;
using MobileLink_Desktop.Views.Dialog;
using MobileLink_Desktop.Views.NoAuth;

namespace MobileLink_Desktop.Service;

public class Session(SocketConnection socketConnection, Navigation navigation, AuthService authService)
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

            Task.WhenAll(tasks.ToArray()).ContinueWith((_) => { ShowInitialLayout(true); });
        }
    }

    public async Task UpdateTokenAndAuthorize(string token, int idDevice)
    {
        var localStorage = new LocalStorage();
        var localStorageContent = localStorage.GetStorage();
        localStorageContent ??= new LocalStorageContent();
        localStorageContent.Token = token;
        localStorageContent.IdDevice = idDevice;
        if (localStorageContent.DefaultReceivingFolder == null)
        {
            Dispatcher.UIThread.Post(async () =>
                {
                    var popUpWindow = new DialogLayout()
                    {
                        Content = new SelectFolderDialog("Selecionar Pasta",
                            "Selecione a pasta padrão para receber as transferências")
                    };
                    var result = await popUpWindow.ShowDialog<string?>(App.GetMainWindow() ?? new Window());
                    localStorageContent.DefaultReceivingFolder =
                        result ?? Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                    localStorage.SetStorage(localStorageContent);
                    VerifyLogIn(true);
                },
                DispatcherPriority.Background);
        }
    }

    private void ShowInitialLayout(bool authorized)
    {
        if (!authorized)
        {
            Dispatcher.UIThread.Post(() => { navigation.UpdateWindow(new NoAuthLayout(), new LoginRegister()); },
                DispatcherPriority.Background);
            return;
        }

        Dispatcher.UIThread.Post(
            () => { navigation.UpdateWindow(new DialogLayout(), new Transference()); },
            DispatcherPriority.Background);
    }
}