using System.ComponentModel;
using System.Threading.Tasks;
using MobileLink_Desktop.Service;

namespace MobileLink_Desktop.ViewModels.NoAuth;

public class EmailValidationViewModel(NavigationService navigationService) : BaseViewModel
{
    private string _code = string.Empty;

    public string Code
    {
        get => _code;
        set
        {
            _code = value;
            NotifyPropertyChanged(Code); 
        }
    }

    public async Task SubmitVerificationCode()
    {
        //TODO call verification controller Auth/verifyCode
    }
    public void Voltar()
    {
        navigationService.NavigateToBack();
    }
}