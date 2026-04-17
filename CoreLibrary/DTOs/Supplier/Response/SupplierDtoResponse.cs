namespace CoreLibrary.DTOs.Supplier.Response;

public class SupplierDtoResponse
{
    public int SupplierId { get; set; }
    public string CompanyName { get; set; } = null!;
    public string? ContactName { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
}