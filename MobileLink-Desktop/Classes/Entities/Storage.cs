namespace MobileLink_Desktop.Entities;

public class Storage
{ 
    public int IdDevice { get; set; }
    public User User { get; set; }
    public long StorageLimitBytes { get; set; }
    public long UsedStorageBytes { get; set; }
}