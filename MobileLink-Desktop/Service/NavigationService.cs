using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using MobileLink_Desktop.ViewModels;

namespace MobileLink_Desktop.Service;



public class NavigationService
{
    private readonly IServiceProvider serviceProvider = App.AppServiceProvider;

    private readonly Stack<UserControl> stackNavigation = new();

    private ContentControl _contentControl = new();

    public void Initialize(ContentControl contentControl)
    {
        this._contentControl = contentControl;
    }

    public void NavigateTo(BaseViewModel viewModel, UserControl tView)
    {
        tView.DataContext = viewModel;
        stackNavigation.Push(tView);
        _contentControl.Content = tView;
    }

    public void NavigateToRoot(BaseViewModel viewModel, UserControl tView)
    {
        stackNavigation.Clear();
        NavigateTo(viewModel, tView);
    }

    public void NavigateToBack()
    {
        if (stackNavigation.Count > 1)
        {
            stackNavigation.Pop();
        }
        var window = stackNavigation.Pop();

        stackNavigation.Push(window);
        _contentControl.Content = window;
    }
}