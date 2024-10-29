using System;

namespace MobileLink_Desktop.Entities;

public class AccessLog
{
    public int IdAccessLog { get; set; }
    public User User { get; set; }
    public Device Device { get; set; }
    public DateTime Date { get; set; }
    public string AccessLocation { get; set; }
}