using System;

namespace MobileLink_Desktop.Entities;

public class DeviceToken
{
    public string Id { get; set; }
    public long IdDevice { get; set; }
    public string Token { get; set; }
    public DateTime InsertionDate { get; set; }
}