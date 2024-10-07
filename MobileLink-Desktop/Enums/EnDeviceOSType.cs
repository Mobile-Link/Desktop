using System.ComponentModel;

namespace MobileLink_Desktop.Enums;

public enum EnDeviceOSType
{
    [Description("Linux")]
    Linux = 1,
    
    [Description("Windows")]
    Windows = 2,
    
    [Description("Android")]
    Android = 3,
    
    [Description("IOS")]
    IOS = 4,
    
    [Description("MacOS")]
    MacOS = 5,
}