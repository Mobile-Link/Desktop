using System.ComponentModel;
using MobileLink_Desktop.Utils;

namespace MobileLink_Desktop.ViewModels.NoAuth;

public class LoginViewModel: INotifyPropertyChanged
{
    private SocketConnection _connection;
    public LoginViewModel(SocketConnection connection)
    {
        _connection = connection;
   
    }

    public void LogInTest()
    {
        
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
}