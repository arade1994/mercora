using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mercora.Infrastructure.Persistence.Models;

[Index("RoleName", Name = "UX_Roles_RoleName", IsUnique = true)]
public partial class Roles
{
    [Key]
    public int RoleId { get; set; }

    [StringLength(50)]
    public string RoleName { get; set; } = null!;

    [Precision(3)]
    public DateTime CreatedAtUtc { get; set; }

    [InverseProperty("Role")]
    public virtual ICollection<UserRoles> UserRoles { get; set; } = new List<UserRoles>();
}
