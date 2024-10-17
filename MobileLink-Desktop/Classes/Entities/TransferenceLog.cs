using MobileLink_Desktop.Enums;

namespace MobileLink_Desktop.Entities;

public class TransferenceLog
{
    public int idTransferenceLog { get; set; } 
    public Transference Transference { get; set; }
    public EnStatusType EnStatusType { get; set; }
}