using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MobileLink_Desktop.Enums;

namespace MobileLink_Desktop.Entities;

public class Device
{
    public int IdDevice { get; set; }
    public User User { get; set; }
    public int IdUser { get; set; }
    public bool IsDeleted { get; set; }
    public string LastLocation { get; set; }
    public long AvailableSpace { get; set; }
    public long OccupiedSpace { get; set; }
    public string Name { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime AlterationDate { get; set; }
    public EnDeviceOSType EnDeviceOs { get; set; }
}