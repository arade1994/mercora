using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mercora.Infrastructure.Persistence.Models;

[Index("IsActive", "Name", Name = "IX_Categories_IsActive_Name")]
[Index("Slug", Name = "UX_Categories_Slug", IsUnique = true)]
public partial class Categories
{
    [Key]
    public int CategoryId { get; set; }

    [StringLength(120)]
    public string Name { get; set; } = null!;

    [StringLength(160)]
    public string Slug { get; set; } = null!;

    public int? ParentCategoryId { get; set; }

    public bool IsActive { get; set; }

    [Precision(3)]
    public DateTime CreatedAtUtc { get; set; }

    [Precision(3)]
    public DateTime UpdatedAtUtc { get; set; }

    [InverseProperty("ParentCategory")]
    public virtual ICollection<Categories> InverseParentCategory { get; set; } = new List<Categories>();

    [ForeignKey("ParentCategoryId")]
    [InverseProperty("InverseParentCategory")]
    public virtual Categories? ParentCategory { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Category")]
    public virtual ICollection<Products> Product { get; set; } = new List<Products>();
}
