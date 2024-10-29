using System;
using MobileLink_Desktop.Enums;

namespace MobileLink_Desktop.Entities;

public class History
{
    public int IdHistory { get; set; }
    public User User { get; set; }
    public int IdUser { get; set; }
    public Device Device { get; set; }
    public int IdDevice { get; set; }
    public EnActionsType EnAction { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
}