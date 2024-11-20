using System;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace MobileLink_Desktop.ViewModels.Dialog;

public class SwalDialogViewModel: DialogViewModel<bool>
{
    public SwalDialogViewModel(string confirmText, string title, string? bodyText, string? cancelText = null,
        ContentControl? bodyContent = null)
    {
        _title = title;
        _bodyText = bodyText;
        _confirmText = confirmText;
        _cancelText = cancelText;
        _bodyText = bodyText;
    }
    private string? _cancelText { get; set; }
    private string _confirmText { get; set; }
    private string _title { get; set; }
    private string? _bodyText { get; set; }
    private ContentControl? _bodyContent { get; set; }//TODO

    public bool CancelButtonVisible { get; set; }

    public string? BodyText
    {
        get => _bodyText;
        set
        {
            _bodyText = value;
            NotifyPropertyChanged(nameof(BodyText));
        }
    }
    public string ConfirmText
    {
        get => _confirmText;
        set
        {
            _confirmText = value;
            NotifyPropertyChanged(nameof(ConfirmText));
        }
    }
    public string? CancelText
    {
        get => _cancelText;
        set
        {
            _cancelText = value;
            CancelButtonVisible = value != null;
            NotifyPropertyChanged(nameof(CancelText));
        }
    }
    
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            NotifyPropertyChanged(nameof(Title));
        }
    }


    public void CancelClick()
    {
        Close(false);
    }
    public void ConfirmClick()
    {
        Close(true);
    }
}