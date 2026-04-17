namespace CoreLibrary.DTOs.Order.Requests;

public class OrderAddDtoRequest
{
    public string CustomerId { get; set; } = null!;
    public int EmployeeId { get; set; } 
    public string? ShipAddress { get; set; }
    public string? ShipCity { get; set; }

    public int CreatedBy { get; set; }



    public List<OrderDetailRequest> Details { get; set; } = new();
}

public class OrderDetailRequest
{
    public int ProductId { get; set; }
    public decimal UnitPrice { get; set; }
    public short Quantity { get; set; }
    public float Discount { get; set; }
}