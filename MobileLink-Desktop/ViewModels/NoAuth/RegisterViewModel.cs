using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using MobileLink_Desktop.Service;
using MobileLink_Desktop.Utils;
using MobileLink_Desktop.Views.NoAuth;

namespace MobileLink_Desktop.ViewModels.NoAuth;

public class RegisterViewModel(ServerAPI api, NavigationService navigationService) : BaseViewModel
{
    private string _email = string.Empty;

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            NotifyPropertyChanged(Email); 
        }
    }
    public async Task SubmitValidateEmail()
    {
        api.SendCode(_email).ContinueWith((sendTask) =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                var viewModel = App.AppServiceProvider.GetRequiredService<EmailValidationViewModel>();
                navigationService.NavigateTo(viewModel, new EmailValidation());
            }, DispatcherPriority.Background);
        });

    }
}