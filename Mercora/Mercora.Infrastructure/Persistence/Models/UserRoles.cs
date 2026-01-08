using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Mercora.Infrastructure.Persistence.Models;

[PrimaryKey("UserId", "RoleId")]
[Index("RoleId", Name = "IX_UserRoles_RoleId")]
[Index("UserId", Name = "IX_UserRoles_UserId")]
public partial class UserRoles
{
    [Key]
    public int UserId { get; set; }

    [Key]
    public int RoleId { get; set; }

    [Precision(3)]
    public DateTime AssignedAtUtc { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("UserRoles")]
    public virtual Roles Role { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("UserRoles")]
    public virtual Users User { get; set; } = null!;
}
