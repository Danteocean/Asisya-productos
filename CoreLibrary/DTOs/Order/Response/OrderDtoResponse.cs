namespace CoreLibrary.DTOs.Order.Response;

public class OrderDtoResponse
{
    public int OrderId { get; set; }
    public string CustomerId { get; set; } = null!;
    public string EmployeeName { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public decimal Total { get; set; }
}