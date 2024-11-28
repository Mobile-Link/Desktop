using System.ComponentModel;

namespace MobileLink_Desktop.Enums;

public enum EnDeviceOSType
{
    [Description("Linux")]
    Linux,
    
    [Description("Windows")]
    Windows,
    
    [Description("Android")]
    Android,
    
    [Description("IOS")]
    IOS,
    
    [Description("MacOS")]
    MacOS,
}