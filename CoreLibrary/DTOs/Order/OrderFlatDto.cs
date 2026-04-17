namespace CoreLibrary.DTOs.Order;

public class OrderFlatDto
{
    public int OrderId { get; set; }
    public string CustomerId { get; set; } = null!;
    public string EmployeeName { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public string? ShipAddress { get; set; }

    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public decimal UnitPrice { get; set; }
    public short Quantity { get; set; }
    public float Discount { get; set; }
}