namespace CoreLibrary.DTOs.Category.Requests;

public class CategoryAddDtoRequest
{
    public string CategoryName { get; set; } = null!;
    public string? Description { get; set; }
    public byte[]? Picture { get; set; }

    public int CreatedBy { get; set; }
}