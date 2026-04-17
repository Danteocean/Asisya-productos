using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.DTOs.Product.Requests;

public class ProductAddDtoRequest
{
    [Required]
    [MaxLength(100)]
    public string ProductName { get; set; } = null!;

    public int? SupplierId { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [MaxLength(50)]
    public string? QuantityPerUnit { get; set; }

    public decimal UnitPrice { get; set; }

    public short UnitsInStock { get; set; }

    public short ReorderLevel { get; set; }

    public bool Discontinued { get; set; } = false;

    public int CreatedBy { get; set; }
}