using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mercora.Infrastructure.Persistence.Models;

[Index("ProductId", "IsPrimary", Name = "IX_ProductImages_ProductId_IsPrimary")]
public partial class ProductImages
{
    [Key]
    public int ImageId { get; set; }

    public int ProductId { get; set; }

    [StringLength(1000)]
    public string Url { get; set; } = null!;

    [StringLength(200)]
    public string? AltText { get; set; }

    public bool IsPrimary { get; set; }

    public int SortOrder { get; set; }

    [Precision(3)]
    public DateTime CreatedAtUtc { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("ProductImages")]
    public virtual Products Product { get; set; } = null!;
}
