using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Mercora.Infrastructure.Persistence.Models;

[Index("ReorderPoint", "QuantityOnHand", Name = "IX_Inventory_LowStock")]
public partial class Inventory
{
    [Key]
    public int VariantId { get; set; }

    public int QuantityOnHand { get; set; }

    public int QuantityReserved { get; set; }

    public int ReorderPoint { get; set; }

    [Precision(3)]
    public DateTime UpdatedAtUtc { get; set; }

    [ForeignKey("VariantId")]
    [InverseProperty("Inventory")]
    public virtual ProductVariants Variant { get; set; } = null!;
}
