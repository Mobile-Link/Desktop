namespace MobileLink_Desktop.Entities;

public class Transference
{ 
    public int idTranference { get; set; }
    public User User { get; set; }
    public Device DeviceOrigin { get; set; }
    public Device DeviceDestination { get; set; }
    public string FileExtention { get; set; }
    public string FileName { get; set; }
    public long Size { get; set; }
    public string DestinationPath { get; set; }
}