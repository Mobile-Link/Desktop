using System;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using MobileLink_Desktop.Views.NoAuth;

namespace MobileLink_Desktop.ViewModels.NoAuth;

public class LoginRegisterViewModel : BaseViewModel
{
    public UserControl LoginControl { get; } = new Login();
    public UserControl RegisterControl { get; } = new Register(){};
    
    
    private GridLength _widthLogin = new GridLength(3, GridUnitType.Star);
    private bool _registerSelected = false;
    private GridLength _widthRegister = new GridLength(2, GridUnitType.Star);
    public GridLength WidthLogin
    {
        get => _widthLogin;
        set
        {
            _widthLogin = value;
            NotifyPropertyChanged(nameof(WidthLogin));
        }
    }
    public GridLength WidthRegister
    {
        get => _widthRegister;
        set
        {
            _widthRegister = value;
            NotifyPropertyChanged(nameof(WidthRegister));
        }
    }
    public bool RegisterSelected
    {
        get => _registerSelected;
        set
        {
            _registerSelected = value;
            NotifyPropertyChanged(nameof(RegisterSelected));
        }
    }
    public void ToggleSelector()
    {
        (WidthLogin, WidthRegister) = (WidthRegister, WidthLogin);
        RegisterSelected = !RegisterSelected;
    }
}