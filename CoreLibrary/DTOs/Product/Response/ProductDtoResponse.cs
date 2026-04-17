namespace CoreLibrary.DTOs.Asisya.Response;

public class ProductDtoResponse
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public int SupplierId { get; set; }

    public string QuantityPerUnit { get; set; } = string.Empty;

    public decimal UnitPrice { get; set; }

    public short UnitsInStock { get; set; }

    public short ReorderLevel { get; set; }

    public bool Discontinued { get; set; }

    public byte[]? CategoryPicture { get; set; } // Foto de la categoría
}