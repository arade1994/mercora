using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Mercora.Infrastructure.Persistence.Models;

[Index("ProductId", "IsActive", Name = "IX_ProductVariants_ProductId_IsActive")]
[Index("Sku", Name = "UX_ProductVariants_Sku", IsUnique = true)]
public partial class ProductVariants
{
    [Key]
    public int VariantId { get; set; }

    public int ProductId { get; set; }

    [StringLength(64)]
    public string Sku { get; set; } = null!;

    [StringLength(120)]
    public string? VariantName { get; set; }

    [StringLength(60)]
    public string? Color { get; set; }

    [StringLength(60)]
    public string? Size { get; set; }

    [Column(TypeName = "decimal(19, 4)")]
    public decimal PriceDelta { get; set; }

    public bool IsActive { get; set; }

    [Precision(3)]
    public DateTime CreatedAtUtc { get; set; }

    [Precision(3)]
    public DateTime UpdatedAtUtc { get; set; }

    [InverseProperty("Variant")]
    public virtual ICollection<CartItems> CartItems { get; set; } = new List<CartItems>();

    [InverseProperty("Variant")]
    public virtual Inventory? Inventory { get; set; }

    [InverseProperty("Variant")]
    public virtual ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();

    [ForeignKey("ProductId")]
    [InverseProperty("ProductVariants")]
    public virtual Products Product { get; set; } = null!;
}
