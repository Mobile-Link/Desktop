using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using MobileLink_Desktop.Utils;

namespace MobileLink_Desktop.ViewModels.Auth;

public class TransferenceViewModel : BaseViewModel
{
    private string _selectedFileName;
    private string _statusTransference;
    private bool _canSendFile = false; 
    
    private readonly SocketConnection _socketConnection;
    
    public TransferenceViewModel(SocketConnection socketConnection)
    {
        _socketConnection = socketConnection;
    }
    
    public string SelectedFileName
    {
        get => _selectedFileName;
        set
        {
            _selectedFileName = value;
            CanSendFile = !string.IsNullOrEmpty(value);
            NotifyPropertyChanged(nameof(SelectedFileName));
        }
    }

    public string StatusTransference
    {
        get => _statusTransference;
        set
        {
            _statusTransference = value;
            NotifyPropertyChanged(nameof(StatusTransference));
        }
    }
    
    public void UpdateStatusTransference(string status)
    {
        StatusTransference = status;
    }

    public async void SelectFile()
    {
        var dialog = new OpenFileDialog
        {
            Title = "Selecione um arquivo",
            AllowMultiple = false
        };
        
        var result = await dialog.ShowAsync(new Window());
        if (result != null && result.Length > 0)
        {
            SelectedFileName = result[0];
        }
    }
    
    public async void SendFile()
    {
        
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
}