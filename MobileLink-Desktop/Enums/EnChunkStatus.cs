using System.ComponentModel;

namespace MobileLink_Desktop.Enums;

public enum EnChunkStatus
{
    [Description("Pending")]
    Pending = 1,
    
    [Description("Received")]
    Received = 2,
    
    [Description("Error")]
    Error = 3
}