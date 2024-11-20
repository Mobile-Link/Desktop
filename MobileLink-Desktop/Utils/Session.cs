using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Microsoft.VisualBasic.FileIO;
using MobileLink_Desktop.Classes;
using MobileLink_Desktop.Entities;
using MobileLink_Desktop.Interfaces;
using MobileLink_Desktop.Service.ApiServices;
using MobileLink_Desktop.Utils;
using MobileLink_Desktop.ViewModels.Dialog;
using MobileLink_Desktop.Views.Dialog;
using MobileLink_Desktop.Views.NoAuth;
using Transference = MobileLink_Desktop.Views.Auth.Transference;

namespace MobileLink_Desktop.Service;

public class Session(SocketConnection socketConnection, Navigation navigation, AuthService authService, TransferenceHandler transferenceHandler)
{
    public async void VerifyLogIn(bool openWindow) //change name
    {
        var localStorage = new LocalStorage();
        var storageContent = localStorage.GetStorage();
        if (storageContent == null || storageContent?.Token == null || storageContent?.IdDevice == null)
        {
            ShowInitialLayout(false);
            return;
        }

        long availableSpace = 0, occupiedSpace = 0;
        foreach (var driveInfo in DriveInfo.GetDrives())
        {
            if (!new [] { DriveType.Fixed, DriveType.Removable }.Contains(driveInfo.DriveType))
            {
                continue;
            }
            availableSpace += driveInfo.TotalFreeSpace;
            occupiedSpace += driveInfo.TotalSize - driveInfo.TotalFreeSpace;
        }
        var result = await authService.UpdateDeviceInformation(storageContent.IdDevice ?? 0, availableSpace, occupiedSpace);
        if (!result.IsSuccessStatusCode)
        {
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                storageContent.Token = null;
                localStorage.SetStorage(storageContent);
                ShowInitialLayout(false);
            }
            if (new []{HttpStatusCode.Gone, HttpStatusCode.NotFound}.Contains(result.StatusCode))
            {
                Dispatcher.UIThread.Post(() =>
                {
                    navigation.ShowDialog(new SwalDialog(), new SwalDialogViewModel(
                        "Ok",
                        "Este dispositivo foi excluído",
                        "Logue novamente para registrar este dispositivo."
                    )).ContinueWith((a) => //TODO GET BOOOL directly i dunno why is returning task
                    {
                        ShowInitialLayout(false);
                    });
                    storageContent.Token = null; storageContent.IdDevice = null;
                    storageContent.DefaultReceivingFolder = null;
                    localStorage.SetStorage(storageContent);
                });
                return;
            }

            if (result.StatusCode == HttpStatusCode.NotAcceptable)
            {
                navigation.ShowDialog(new SwalDialog(), new SwalDialogViewModel(
                    "Ok",
                    "Ocorreu um erro com o serviço",
                    "Por favor, tente novamente mais tarde. [E]: 406"
                ));
                return;
            }
        }

        var body = await result.Content.ReadAsStringAsync();
        var device = JsonSerializer.Deserialize<Device>(body);
        //TODO get device information tha comes here and store it
        if (device == null)
        {
            ShowInitialLayout(false);
            return;
        }

        if (storageContent.OpenWindowOnStartUp || openWindow)
        {
            
            InitializeAuthorizedServices().ContinueWith(_ =>
            {
                ShowInitialLayout(true);
            });
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
                    var dialog = navigation.ShowDialog(
                        new SelectFolderDialog(), 
                        new SelectFolderDialogViewModel(
                            "Selecionar Pasta", 
                            "Selecione a pasta padrão para receber as transferências"
                        )
                    );
                    var result = await dialog;
                    localStorageContent.DefaultReceivingFolder =
                        result ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "MobileLink");
                    localStorage.SetStorage(localStorageContent);
                    VerifyLogIn(true);
                },
                DispatcherPriority.Background);
            return;
        }
        localStorage.SetStorage(localStorageContent);
        VerifyLogIn(true);
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

    private async Task InitializeAuthorizedServices()
    {
        var tasks = new List<Task>();
        if (socketConnection.StatusType != EnServerconnectionStatusType.Connected)
        {
            tasks.Add(socketConnection.Connect());
        }
        transferenceHandler.CheckTransfersNotReceived().ContinueWith((_) => {});
        transferenceHandler.CheckTransfersNotSent().ContinueWith((_) => {});
        await Task.WhenAll(tasks);
    }
}