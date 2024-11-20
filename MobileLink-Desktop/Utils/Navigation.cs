using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using MobileLink_Desktop.Entities;
using MobileLink_Desktop.ViewModels;
using MobileLink_Desktop.ViewModels.Dialog;

namespace MobileLink_Desktop.Service;



public class Navigation
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

    public void UpdateWindow(Window window, UserControl content)
    {
        App.ChangeWindow(window);
        Initialize(window);
        NavigateToRoot(content);
    }

    public async Task<T> ShowDialog<T>(UserControl control, DialogViewModel<T> viewModel, Window? window = null)
    {
        if (window == null)
        {
            window = new DialogLayout();
        }
        var tcs = new TaskCompletionSource<T>();
        control.DataContext = viewModel;
        window.Content = control;
        window.Show();
        viewModel.CloseDialog += ((sender, e) =>
        {
            window.Close();
            tcs.SetResult(e); 
        });
        return await tcs.Task;
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