using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mercora.Infrastructure.Persistence.Models;

[Index("IsPublished", "CreatedAtUtc", Name = "IX_Products_IsPublished_CreatedAtUtc", IsDescending = new[] { false, true })]
[Index("Slug", Name = "UX_Products_Slug", IsUnique = true)]
public partial class Products
{
    [Key]
    public int ProductId { get; set; }

    [StringLength(200)]
    public string Name { get; set; } = null!;

    [StringLength(220)]
    public string Slug { get; set; } = null!;

    public string? Description { get; set; }

    [Column(TypeName = "decimal(19, 4)")]
    public decimal BasePrice { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string CurrencyCode { get; set; } = null!;

    public bool IsPublished { get; set; }

    public bool IsDeleted { get; set; }

    [Precision(3)]
    public DateTime CreatedAtUtc { get; set; }

    [Precision(3)]
    public DateTime UpdatedAtUtc { get; set; }

    [InverseProperty("Product")]
    public virtual ProductImages? ProductImages { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<ProductVariants> ProductVariants { get; set; } = new List<ProductVariants>();

    [ForeignKey("ProductId")]
    [InverseProperty("Product")]
    public virtual ICollection<Categories> Category { get; set; } = new List<Categories>();
}
