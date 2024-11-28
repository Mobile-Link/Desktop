using System.ComponentModel;

namespace MobileLink_Desktop.Enums;

public enum EnStatusType
{
    [Description("Not started")]
    NotStarted,
    
    [Description("In progress")]
    InProgress,
    
    [Description("In cloud")]
    InCloud,
    
    [Description("Verifying")]
    Verifying,
    
    [Description("Finished")]
    Finished,
    
    [Description("Error")]
    Error,
    
    [Description("Canceled")]
    Canceled,
}