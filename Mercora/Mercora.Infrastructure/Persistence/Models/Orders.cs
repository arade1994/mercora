using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mercora.Infrastructure.Persistence.Models;

[Index("UserId", "CreatedAtUtc", Name = "IX_Orders_User_CreatedAt", IsDescending = new[] { false, true })]
[Index("OrderNumber", Name = "UX_Orders_OrderNumber", IsUnique = true)]
public partial class Orders
{
    [Key]
    public int OrderId { get; set; }

    public int UserId { get; set; }

    [StringLength(30)]
    public string OrderNumber { get; set; } = null!;

    public byte Status { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string CurrencyCode { get; set; } = null!;

    [Column(TypeName = "decimal(19, 4)")]
    public decimal Subtotal { get; set; }

    [Column(TypeName = "decimal(19, 4)")]
    public decimal DiscountTotal { get; set; }

    [Column(TypeName = "decimal(19, 4)")]
    public decimal TaxTotal { get; set; }

    [Column(TypeName = "decimal(19, 4)")]
    public decimal ShippingTotal { get; set; }

    [Column(TypeName = "decimal(22, 4)")]
    public decimal? GrandTotal { get; set; }

    [Precision(3)]
    public DateTime CreatedAtUtc { get; set; }

    [Precision(3)]
    public DateTime? PaidAtUtc { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();

    [InverseProperty("Order")]
    public virtual ICollection<Payments> Payments { get; set; } = new List<Payments>();

    [ForeignKey("UserId")]
    [InverseProperty("Orders")]
    public virtual Users User { get; set; } = null!;
}
