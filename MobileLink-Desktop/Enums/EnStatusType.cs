using System.ComponentModel;

namespace MobileLink_Desktop.Enums;

public enum EnStatusType
{
    [Description("Not started")]
    NotStarted = 1,
    
    [Description("In progress")]
    InProgress = 2,
    
    [Description("In cloud")]
    InCloud = 3,
    
    [Description("Verifing")]
    Verifing = 4,
    
    [Description("Finished")]
    Finished = 5,
    
    [Description("Error")]
    Error = 6,
    
    [Description("Canceled")]
    Canceled = 7,
}