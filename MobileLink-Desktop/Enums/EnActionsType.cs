using System.ComponentModel;

namespace MobileLink_Desktop.Enums;

public enum EnActionsType
{
    [Description("Deleted device")]
    DeletedDevice = 1,
    
    [Description("Added device")]
    AddedDevice = 2,
    
    [Description("Changed password")]
    ChangedPassword = 3,
    
    [Description("Changed device")]
    ChangedDevice = 4,
    
    [Description("Changed user")]
    ChangedUser = 5,
}