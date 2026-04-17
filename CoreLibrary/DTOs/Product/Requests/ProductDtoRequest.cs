namespace CoreLibrary.DTOs.Asisya.Requests;

public class ProductDtoRequest
{
    public string? Search { get; set; }
    public int? CategoryId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class ProductIdDtoRequest
{
    public int ProductId { get; set; }
}