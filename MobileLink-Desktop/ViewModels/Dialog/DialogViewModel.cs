using System;

namespace MobileLink_Desktop.ViewModels.Dialog;

public class DialogViewModel <T> : BaseViewModel
{
    public event EventHandler<T>? CloseDialog;
    public void Close(T param)
    {
        CloseDialog?.Invoke(this, param);
    }
}