using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace CleanArchitecture.Domain.Entities;

public class PaymentCollection : BaseEntity
{
    [Required]
    public int OrganizationId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string TransactionId { get; set; } = string.Empty;
    
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal BilledAmount { get; set; }
    
    [Required]
    public int TaxId { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal TaxAmount { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal TotalAmount { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string ProductName { get; set; } = string.Empty;
    
    [Required]
    public int Quantity { get; set; } = 1;
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal UnitPrice { get; set; }
    
    [Required]
    public int FeeTaxType { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string PaymentMethod { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(20)]
    public string PaymentStatus { get; set; } = string.Empty;
    
    public JsonDocument? Details { get; set; }
    
    // Navigation properties
    [ForeignKey(nameof(OrganizationId))]
    public virtual Organization Organization { get; set; } = null!;
    
    [ForeignKey(nameof(TaxId))]
    public virtual TaxType Tax { get; set; } = null!;
} 