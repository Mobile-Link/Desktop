using System;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using MobileLink_Desktop.Views.NoAuth;

namespace MobileLink_Desktop.ViewModels.NoAuth;

public class LoginRegisterViewModel : INotifyPropertyChanged
{
    public UserControl LoginControl { get; } = new Login(){ DataContext = App.AppServiceProvider.GetRequiredService<LoginViewModel>()};
    public UserControl RegisterControl { get; } = new Register(){ DataContext = App.AppServiceProvider.GetRequiredService<RegisterViewModel>()};
    
    
    private GridLength _widthLogin = new GridLength(3, GridUnitType.Star);
    private bool _registerSelected = false;
    private GridLength _widthRegister = new GridLength(2, GridUnitType.Star);
    public GridLength WidthLogin
    {
        get => _widthLogin;
        set
        {
            _widthLogin = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WidthLogin)));
        }
    }
    public GridLength WidthRegister
    {
        get => _widthRegister;
        set
        {
            _widthRegister = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WidthRegister)));
        }
    }
    public bool RegisterSelected
    {
        get => _registerSelected;
        set
        {
            _registerSelected = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RegisterSelected)));
        }
    }
    public void ToggleSelector()
    {
        (WidthLogin, WidthRegister) = (WidthRegister, WidthLogin);
        RegisterSelected = !RegisterSelected;
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
}