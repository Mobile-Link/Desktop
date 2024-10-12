using System.ComponentModel;
using System.Threading.Tasks;
using MobileLink_Desktop.Utils;

namespace MobileLink_Desktop.ViewModels.NoAuth;

public class RegisterViewModel: INotifyPropertyChanged
{
    private ServerAPI _api;
    public RegisterViewModel(ServerAPI api)
    {
        _api = api;
    }
    private string _email = string.Empty;

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Email)); 
        }
    }
    public async Task SubmitValidateEmail()
    {
        _api.SendCode(_email).ContinueWith((sendTask) =>
        {
            //TODO send to code verification stack
        });

    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
}