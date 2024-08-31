using System.ComponentModel;

namespace MobileLink_Desktop.ViewModels;

public class MainWindowViewModel: INotifyPropertyChanged
{
    private string _message = "Default text";

    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));       
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
}