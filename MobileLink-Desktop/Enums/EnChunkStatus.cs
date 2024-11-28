using System.ComponentModel;

namespace MobileLink_Desktop.Enums;

public enum EnChunkStatus
{
    [Description("Pending")]
    Pending,
    
    [Description("Received")]
    Received,
    
    [Description("Error")]
    Error
}