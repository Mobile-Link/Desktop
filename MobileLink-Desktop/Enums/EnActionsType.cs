using System.ComponentModel;

namespace MobileLink_Desktop.Enums;

public enum EnActionsType
{
    [Description("Deleted device")]
    DeletedDevice,
    
    [Description("Added device")]
    AddedDevice,
    
    [Description("Changed password")]
    ChangedPassword,
    
    [Description("Changed device")]
    ChangedDevice,
    
    [Description("Changed user")]
    ChangedUser,
}