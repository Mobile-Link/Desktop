using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Microsoft.AspNetCore.SignalR.Client;
using MobileLink_Desktop.Classes;
using MobileLink_Desktop.Entities;
using MobileLink_Desktop.Service.ApiServices;
using MobileLink_Desktop.Utils;
using Newtonsoft.Json;

namespace MobileLink_Desktop.ViewModels.Auth;

public class TransferenceViewModel : BaseViewModel
{
    private IStorageItem? _selectedFile = null;
    private int? _selectedDeviceIndex = null;
    private int _progressTransference;
    private ObservableCollection<Device> _devices = [];
    private bool _canSendFile = false;

    private readonly ConnectionService _connectionService;
    private readonly DeviceService _deviceService;
    private readonly TransferenceService _transferenceService;
    private readonly HubConnection _connection;

    public TransferenceViewModel(DeviceService deviceService,
        ConnectionService connectionService, TransferenceService transferenceService, SocketConnection socketConnection)
    {
        _deviceService = deviceService;
        _connectionService = connectionService;
        _transferenceService = transferenceService;
        _connection = socketConnection.Connection;
        _connection.On<int[]>("UpdateConnectedDevices", PopulateDevices);
        _connectionService.GetConnectedDevices().ContinueWith((taskCon) =>
        {
            PopulateDevices(taskCon.Result.ToArray());
        });
    }

    public ObservableCollection<Device> Devices
    {
        get => _devices;
        set
        {
            _devices = value;
            NotifyPropertyChanged(nameof(Devices));
        }
    }

    public IStorageItem? SelectedFileName
    {
        get => _selectedFile;
        set
        {
            _selectedFile = value;
            CanSendFile = value != null;
            NotifyPropertyChanged(nameof(SelectedFileName));
        }
    }

    public int? SelectedDeviceIndex
    {
        get => _selectedDeviceIndex;
        set
        {
            _selectedDeviceIndex = value;
            NotifyPropertyChanged(nameof(SelectedDeviceIndex));
        }
    }

    public int ProgressTransference
    {
        get => _progressTransference;
        set
        {
            _progressTransference = value;
            NotifyPropertyChanged(nameof(ProgressTransference));
        }
    }

    public void UpdateStatusTransference(int status)
    {
        ProgressTransference = status;
    }

    public void SelectFile()
    {
        var mainWindow = App.GetMainWindow();
        var topLevel = TopLevel.GetTopLevel(mainWindow);
        if (topLevel == null)
        {
            return; //TODO error
        }

        topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Selecione um arquivo",
            AllowMultiple = false
        }).ContinueWith((result) =>
        {
            if (result.Result.Count > 0)
            {
                SelectedFileName = result.Result[0];
            }
        });
    }

    public void SendFile()
    {
        if (_selectedFile == null || _selectedDeviceIndex == null)
        {
            //TODO popup form error
            return;
        }

        Device? device = null;
        device = Devices[_selectedDeviceIndex ?? 0];
        if (device == null)
        {
            //TODO error
            return;
        }

        var length = new System.IO.FileInfo(_selectedFile.Path.AbsolutePath).Length;
        _transferenceService.StartTransference(device.IdDevice, _selectedFile.Path.AbsolutePath, length, "/")
            .ContinueWith(
                (taskStart) =>
                {
                    if (taskStart.Result == null)
                    {
                        //TODO error
                        return;
                    }

                    using (FileStream fs = File.OpenRead(_selectedFile.Path.AbsolutePath))
                    {
                        const int chunkSize = 1024 * 1024;
                        var totalChunks = (int)Math.Ceiling((double)fs.Length / chunkSize);

                        long startByteIndex = 0;
                        var chunkIndex = 0;

                        while (startByteIndex < fs.Length)
                        {
                            var byteArray = new byte[chunkSize];
                            fs.Read(byteArray, 0, chunkSize);
                            _transferenceService.SendFileChunk(taskStart.Result ?? 0, startByteIndex, byteArray).Wait();
                            startByteIndex += chunkSize;
                            chunkIndex++;
                            UpdateStatusTransference((int)Math.Ceiling((double)chunkIndex / totalChunks * 100));
                        }
                    }
                });
    }

    public bool CanSendFile
    {
        get => _canSendFile;
        private set
        {
            _canSendFile = value;
            NotifyPropertyChanged(nameof(CanSendFile));
        }
    }

    private void PopulateDevices(int[] connectedDevices)
    {
        Devices.Clear();
        var storageContent = new LocalStorage().GetStorage();
        if (storageContent?.IdDevice == null)
        {
            //TODO THROW error of make a handler that disponibilizes it globally to not do this each
            return;
        }

        _deviceService.GetUserDevices().ContinueWith((taskUsr) =>
        {
            var devices = taskUsr.Result;
            var userDevices = devices.Where((device) => device.IdDevice != storageContent.IdDevice).ToList();
            if (devices.Count == userDevices.Count)
            {
                
            }
            Devices = new ObservableCollection<Device>(
                userDevices.Where((device) => connectedDevices.Contains(device.IdDevice)).ToList()
            );
        });
    }
}