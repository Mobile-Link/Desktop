using System;

namespace MobileLink_Desktop.Entities;

public class User
{
    public int IdUser { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public DateTime CreationDate { get; set; }
}