using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using MobileLink_Desktop.ViewModels;

namespace MobileLink_Desktop.Service;



public class NavigationService
{
    private readonly Stack<UserControl> _stackNavigation = new();

    private ContentControl _contentControl = new();

    public void Initialize(ContentControl contentControl)
    {
        this._contentControl = contentControl;
    }

    public void NavigateTo(UserControl tView)
    {
        _stackNavigation.Push(tView);
        _contentControl.Content = tView;
    }

    public void NavigateToRoot(UserControl tView)
    {
        _stackNavigation.Clear();
        NavigateTo(tView);
    }

    public void NavigateToBack()
    {
        if (_stackNavigation.Count > 1)
        {
            _stackNavigation.Pop();
        }
        var window = _stackNavigation.Pop();

        _stackNavigation.Push(window);
        _contentControl.Content = window;
    }
}