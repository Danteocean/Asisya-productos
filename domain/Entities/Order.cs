using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("orders")]
public class Order : IEntity
{
    [Key]
    [Column("order_id")]
    public int OrderId { get; set; }

    [Column("customer_id")]
    public string? CustomerId { get; set; }

    [Column("employee_id")]
    public int? EmployeeId { get; set; }

    [Column("order_date")]
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    [Column("required_date")]
    public DateTime? RequiredDate { get; set; }

    [Column("shipped_date")]
    public DateTime? ShippedDate { get; set; }

    [Column("freight")]
    public decimal? Freight { get; set; }

    [Column("ship_name")]
    [MaxLength(40)]
    public string? ShipName { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdateAt { get; set; }

    // Relaciones
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}