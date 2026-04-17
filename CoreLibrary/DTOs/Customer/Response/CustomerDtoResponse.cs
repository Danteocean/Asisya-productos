namespace CoreLibrary.DTOs.Customer.Response;

public class CustomerDtoResponse
{
    public string CustomerId { get; set; } = null!;
    public string CompanyName { get; set; } = null!;
    public string? ContactName { get; set; }
    public string? City { get; set; }
}