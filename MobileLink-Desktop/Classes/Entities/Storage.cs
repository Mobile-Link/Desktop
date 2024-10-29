namespace MobileLink_Desktop.Entities;

public class Storage
{
    public int IdStorage { get; set; }
    public User User { get; set; }
    public long StorageLimitBytes { get; set; }
    public long UsedStorageBytes { get; set; }
}