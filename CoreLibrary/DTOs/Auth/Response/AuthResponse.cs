namespace CoreLibrary.DTOs.Auth.Response;

public class AuthResponse
{
    public int EmployeeId { get; set; }
    public string Username { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string FullName { get; set; } = null!;
}