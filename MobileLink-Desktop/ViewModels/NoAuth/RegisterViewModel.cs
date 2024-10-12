using System.ComponentModel;
using MobileLink_Desktop.Utils;

namespace MobileLink_Desktop.ViewModels.NoAuth;

public class RegisterViewModel: INotifyPropertyChanged
{
    private SocketConnection _connection;
    public RegisterViewModel(SocketConnection connection)
    {
        _connection = connection;
   
    }

    public void RegisterTest()
    {
        
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
}