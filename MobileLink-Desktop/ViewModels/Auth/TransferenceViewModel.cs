using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using MobileLink_Desktop.Utils;

namespace MobileLink_Desktop.ViewModels.Auth;

public class TransferenceViewModel(SocketConnection socketConnection) : BaseViewModel
{
    private string _selectedFileName;
    private string _statusTransference;
    private bool _canSendFile = false; 
    
    private readonly SocketConnection _socketConnection = socketConnection;

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
        var mainWindow = GetTopLevel();
        var topLevel = TopLevel.GetTopLevel(mainWindow);
        if (topLevel == null)
        {
            return;//TODO error
        }

        topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Selecione um arquivo",
            AllowMultiple = false
        }).ContinueWith((result) =>
        {
            if (result.Result.Count > 0)
            {
                SelectedFileName = result.Result[0].Name;
            }
        });
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
    
    private Window? GetTopLevel()
    {
        if (Application.Current?.ApplicationLifetime is ClassicDesktopStyleApplicationLifetime desktopLifetime)
        {
            if (desktopLifetime.MainWindow != null)
            {
                return desktopLifetime.MainWindow;
            }
            if(desktopLifetime.Windows.Count > 0)
            {
                return desktopLifetime.Windows[0];
            }
        }

        return null;
    }
}