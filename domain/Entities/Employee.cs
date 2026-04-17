using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("employees")]
public class Employee : IEntity
{
    [Key]
    [Column("employee_id")]
    public int EmployeeId { get; set; }

    [Required]
    [MaxLength(20)]
    [Column("last_name")]
    public string LastName { get; set; } = null!;

    [Required]
    [MaxLength(10)]
    [Column("first_name")]
    public string FirstName { get; set; } = null!;

    [MaxLength(30)]
    [Column("title")]
    public string? Title { get; set; }

    [Column("birth_date")]
    public DateTime? BirthDate { get; set; }

    [Column("hire_date")]
    public DateTime? HireDate { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("username")]
    public string Username { get; set; } = null!;

    [Required]
    [Column("password_hash")]
    public string PasswordHash { get; set; } = null!;

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("created_by")]
    public int? CreatedBy { get; set; }

    [Column("updated_by")]
    public int? UpdatedBy { get; set; }

  
    [Column("reports_to")]
    public int? ReportsTo { get; set; }

    [ForeignKey("ReportsTo")]
    public virtual Employee? Manager { get; set; }
}