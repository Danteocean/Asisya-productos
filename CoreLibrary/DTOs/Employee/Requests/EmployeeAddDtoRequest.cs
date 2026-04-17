namespace CoreLibrary.DTOs.Employee.Requests;

public class EmployeeAddDtoRequest
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Title { get; set; }

    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!; 
}