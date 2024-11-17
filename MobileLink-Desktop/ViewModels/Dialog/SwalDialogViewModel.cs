using System;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace MobileLink_Desktop.ViewModels.Dialog;

public class SwalDialogViewModel: BaseViewModel
{
    private string? _cancelText { get; set; }
    private string _confirmText { get; set; }
    private string _title { get; set; }
    private string? _bodyText { get; set; }
    private ContentControl? _bodyContent { get; set; }//TODO
    public event EventHandler<bool>? Result;

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
        Result?.Invoke(this, false);
    }
    public void ConfirmClick()
    {
        Result?.Invoke(this, true);
    }
}