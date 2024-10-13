using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using MobileLink_Desktop.Entities;
using MobileLink_Desktop.Service.ApiServices;
using MobileLink_Desktop.Utils;

namespace MobileLink_Desktop.ViewModels.Auth;

public class TransferenceViewModel : BaseViewModel
{
    private IStorageItem? _selectedFile = null;
    private long? _selectedDevice = null;
    private int _progressTransference;
    private ObservableCollection<Device> _devices = [];
    private bool _canSendFile = false;

    private readonly SocketMethods _socketConnection;
    private readonly ConnectionService _connectionService;
    private readonly DeviceService _deviceService;

    public TransferenceViewModel(SocketMethods socketConnection, DeviceService deviceService,
        ConnectionService connectionService)
    {
        _socketConnection = socketConnection;
        _deviceService = deviceService;
        _connectionService = connectionService;
        PopulateDevices();
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
    public long? SelectedDevice
    {
        get => _selectedDevice;
        set
        {
            _selectedDevice = value;
            NotifyPropertyChanged(nameof(SelectedDevice));
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
        if (_selectedFile == null || _selectedDevice == null)
        {
            //TODO popup form error
            return;
        }
        var length = new System.IO.FileInfo(_selectedFile.Path.AbsolutePath).Length;
        _socketConnection.StartTransfer(_selectedDevice ?? 0, _selectedFile.Path.AbsolutePath, length, "/").ContinueWith(
            (taskStart) =>
            {
                var idTransference = 0; //TODO get idTransference or from the socket of http request
                //TODO separate file into chunks and for each chunk call SendPacket(long idTransfer, long startByteIndex, byte[] byteArray)
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

    private void PopulateDevices()
    {
        var userDevices = new List<Device>();
        var connectedDevices = new List<int>();
        Devices.Clear();
        var tasks = new List<Task>
        {
            _deviceService.GetUserDevices().ContinueWith((taskUsr) => { userDevices = taskUsr.Result; }),
            _connectionService.GetConnectedDevices().ContinueWith((taskCon) => { connectedDevices = taskCon.Result; })
        };
        Task.WhenAll(tasks.ToArray()).ContinueWith((taskTasks) =>
        {
            Devices = new ObservableCollection<Device>(
                userDevices.Where((device) => connectedDevices.Contains(device.IdDevice)).ToList()
            );
            if (Devices.Count == 0)
            {
                //temp
                Devices.Add(new Device()
                {
                    Name = "Teste",
                    IdDevice = 123
                });
                //TODO alert no devices
            }
        });
    }
}