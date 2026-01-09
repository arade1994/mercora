using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mercora.Infrastructure.Persistence.Models;

[Index("CartId", Name = "IX_CartItems_CartId")]
[Index("CartId", "VariantId", Name = "UX_CartItems_Cart_Variant", IsUnique = true)]
public partial class CartItems
{
    [Key]
    public int CartItemId { get; set; }

    public int CartId { get; set; }

    public int VariantId { get; set; }

    public int Quantity { get; set; }

    [Precision(3)]
    public DateTime AddedAtUtc { get; set; }

    [Precision(3)]
    public DateTime UpdatedAtUtc { get; set; }

    [ForeignKey("CartId")]
    [InverseProperty("CartItems")]
    public virtual Carts Cart { get; set; } = null!;

    [ForeignKey("VariantId")]
    [InverseProperty("CartItems")]
    public virtual ProductVariants Variant { get; set; } = null!;
}
