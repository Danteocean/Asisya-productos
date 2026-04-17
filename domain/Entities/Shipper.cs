using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("shippers")]
public class Shipper : IEntity
{
    [Key]
    public int ShipperId { get; set; }

    [Required]
    [MaxLength(100)]
    public string CompanyName { get; set; } = null!;

    [MaxLength(25)]
    public string? Phone { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}