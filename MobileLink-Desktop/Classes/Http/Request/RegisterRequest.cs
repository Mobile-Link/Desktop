namespace MobileLink_Desktop.Classes.Http.Request;

public class RegisterRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Username { get; set; }
    public string Code { get; set; }
    public string DeviceName { get; set; }
}