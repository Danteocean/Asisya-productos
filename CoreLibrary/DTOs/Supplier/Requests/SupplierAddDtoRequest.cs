namespace CoreLibrary.DTOs.Supplier.Requests;

public class SupplierAddDtoRequest
{
    public string CompanyName { get; set; } = null!;
    public string? ContactName { get; set; }
    public string? ContactTitle { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? Phone { get; set; }
}