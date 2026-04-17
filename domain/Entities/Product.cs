using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("products")]
public class Product : IEntity
{
    [Key]
    [Column("product_id")]
    public int ProductId { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("product_name")]
    public string ProductName { get; set; } = null!;

    [Column("supplier_id")]
    public int? SupplierId { get; set; }

    [Column("category_id")]
    public int? CategoryId { get; set; }

    [MaxLength(50)]
    [Column("quantity_per_unit")]
    public string? QuantityPerUnit { get; set; }

    [Column("unit_price", TypeName = "numeric(12,4)")]
    public decimal UnitPrice { get; set; }

    [Column("units_in_stock")]
    public short UnitsInStock { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column("created_by")]
    public int? CreatedBy { get; set; }

    [Column("updated_by")]
    public int? UpdatedBy { get; set; }

    [ForeignKey("CategoryId")]
    public virtual Category? Category { get; set; }



}