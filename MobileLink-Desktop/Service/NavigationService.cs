using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;

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

    public void NavigateTo<TViewModel>(UserControl TView) where TViewModel : class
    {
        var viewModel = serviceProvider.GetRequiredService<TViewModel>();
        TView.DataContext = viewModel;
        stackNavigation.Push(TView);
        _contentControl.Content = TView;
    }

    public void NavigateToRoot<TViewModel>(UserControl TView) where TViewModel : class
    {
        stackNavigation.Clear();
        NavigateTo<TViewModel>(TView);
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