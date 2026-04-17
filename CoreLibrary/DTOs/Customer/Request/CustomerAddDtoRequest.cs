namespace CoreLibrary.DTOs.Customer.Request;

public class CustomerAddDtoRequest
{
    public string CustomerId { get; set; } = null!;
    public string CompanyName { get; set; } = null!;
    public string? ContactName { get; set; }
    public string? City { get; set; }
    public string? Phone { get; set; }
}