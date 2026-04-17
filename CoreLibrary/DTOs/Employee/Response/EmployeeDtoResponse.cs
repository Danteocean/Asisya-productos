namespace CoreLibrary.DTOs.Employee.Response;

public class EmployeeDtoResponse
{
    public int EmployeeId { get; set; }
    public string FullName { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string? Title { get; set; }
    public bool IsActive { get; set; }
}