using System;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace MobileLink_Desktop.ViewModels.Dialog;

public class SelectFolderDialogViewModel: BaseViewModel
{
    private string? ResultFolder { get; set; }
    private string _instructionText { get; set; }
    private string _buttonText { get; set; }
    public event EventHandler<string?>? FolderSelected;
    public string InstructionText
    {
        get => _instructionText;
        set
        {
            _instructionText = value;
            NotifyPropertyChanged(nameof(InstructionText));
        }
    }
    
    public string ButtonText
    {
        get => _buttonText;
        set
        {
            _buttonText = value;
            NotifyPropertyChanged(nameof(ButtonText));
        }
    }

    public void SelectFolder()
    {
        var mainWindow = App.GetMainWindow();
        var topLevel = TopLevel.GetTopLevel(mainWindow);
        if (topLevel == null)
        {
            return; //TODO error
        }

        topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            Title = "Selecione um diretório",
            AllowMultiple = false
        }).ContinueWith((result) =>
        {
            if (result.Result.Count > 0)
            {
                ResultFolder = result.Result[0].TryGetLocalPath();
            }
        });
    }

    public void Cancel()
    {
        FolderSelected?.Invoke(this, null);
    }
    public void Confirm()
    {
        FolderSelected?.Invoke(this, ResultFolder);
    }
}