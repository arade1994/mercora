using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mercora.Infrastructure.Persistence.Models;

[Index("OrderId", "CreatedAtUtc", Name = "IX_Payments_OrderId_CreatedAtUtc", IsDescending = new[] { false, true })]
public partial class Payments
{
    [Key]
    public int PaymentId { get; set; }

    public int OrderId { get; set; }

    [StringLength(50)]
    public string Provider { get; set; } = null!;

    [StringLength(200)]
    public string? ProviderRef { get; set; }

    public byte Status { get; set; }

    [Column(TypeName = "decimal(19, 4)")]
    public decimal Amount { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string CurrencyCode { get; set; } = null!;

    [StringLength(100)]
    public string? FailureCode { get; set; }

    [StringLength(400)]
    public string? FailureMessage { get; set; }

    [Precision(3)]
    public DateTime CreatedAtUtc { get; set; }

    [Precision(3)]
    public DateTime UpdatedAtUtc { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("Payments")]
    public virtual Orders Order { get; set; } = null!;
}
