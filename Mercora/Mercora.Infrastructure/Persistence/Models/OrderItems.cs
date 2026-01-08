using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Mercora.Infrastructure.Persistence.Models;

[Index("OrderId", Name = "IX_OrderItems_OrderId")]
public partial class OrderItems
{
    [Key]
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int VariantId { get; set; }

    [StringLength(64)]
    public string SkuSnapshot { get; set; } = null!;

    [StringLength(200)]
    public string ProductNameSnap { get; set; } = null!;

    [StringLength(120)]
    public string? VariantNameSnap { get; set; }

    [Column(TypeName = "decimal(19, 4)")]
    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(30, 4)")]
    public decimal? LineTotal { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderItems")]
    public virtual Orders Order { get; set; } = null!;

    [ForeignKey("VariantId")]
    [InverseProperty("OrderItems")]
    public virtual ProductVariants Variant { get; set; } = null!;
}
