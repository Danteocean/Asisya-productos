using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("customers")]
public class Customer : IEntity
{
    [Key]
    [Column("customer_id")]
    [MaxLength(5)]
    public string CustomerId { get; set; } = null!;

    [Required]
    [MaxLength(40)]
    [Column("company_name")]
    public string CompanyName { get; set; } = null!;

    [MaxLength(30)]
    [Column("contact_name")]
    public string? ContactName { get; set; }

    [MaxLength(30)]
    [Column("contact_title")]
    public string? ContactTitle { get; set; }

    [Column("address")]
    public string? Address { get; set; }

    [Column("city")]
    public string? City { get; set; }

    [Column("phone")]
    public string? Phone { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdateAt { get; set; }
}