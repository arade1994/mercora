using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Mercora.Infrastructure.Persistence.Models;

[Index("UserId", "Status", Name = "IX_Carts_UserId_Status")]
public partial class Carts
{
    [Key]
    public int CartId { get; set; }

    public int UserId { get; set; }

    public byte Status { get; set; }

    [Precision(3)]
    public DateTime CreatedAtUtc { get; set; }

    [Precision(3)]
    public DateTime UpdatedAtUtc { get; set; }

    [InverseProperty("Cart")]
    public virtual ICollection<CartItems> CartItems { get; set; } = new List<CartItems>();

    [ForeignKey("UserId")]
    [InverseProperty("Carts")]
    public virtual Users User { get; set; } = null!;
}
