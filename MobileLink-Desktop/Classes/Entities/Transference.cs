using System;
using MobileLink_Desktop.Enums;

namespace MobileLink_Desktop.Entities;

public class Transference
{
    public int IdTransference { get; set; }
    public User User { get; set; }
    public int IdUser { get; set; }
    public Device DeviceOrigin { get; set; }
    public int IdDeviceOrigin { get; set; }
    public Device DeviceDestination { get; set; }
    public int IdDeviceDestination { get; set; }
    public string FilePath { get; set; }
    public long Size { get; set; }
    public string DestinationPath { get; set; }
    public EnStatusType EnStatus { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime UpdateDate { get; set; }

}