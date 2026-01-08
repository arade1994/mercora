using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Mercora.Infrastructure.Persistence.Models;

[Index("Email", Name = "UX_Users_Email", IsUnique = true)]
public partial class Users
{
    [Key]
    public int UserId { get; set; }

    [StringLength(256)]
    public string Email { get; set; } = null!;

    [MaxLength(256)]
    public byte[] PasswordHash { get; set; } = null!;

    [MaxLength(128)]
    public byte[] PasswordSalt { get; set; } = null!;

    [StringLength(100)]
    public string? FirstName { get; set; }

    [StringLength(100)]
    public string? LastName { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }

    public bool IsActive { get; set; }

    [Precision(3)]
    public DateTime CreatedAtUtc { get; set; }

    [Precision(3)]
    public DateTime UpdatedAtUtc { get; set; }

    [InverseProperty("User")]
    public virtual Carts? Carts { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();

    [InverseProperty("User")]
    public virtual ICollection<UserRoles> UserRoles { get; set; } = new List<UserRoles>();
}
