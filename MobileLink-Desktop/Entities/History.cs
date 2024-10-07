using MobileLink_Desktop.Enums;

namespace MobileLink_Desktop.Entities;

public class History
{
    public int idHistory { get; set; }
    public User User { get; set; }
    public Device Device { get; set; }
    public EnActionsType EnActionType { get; set; }
}