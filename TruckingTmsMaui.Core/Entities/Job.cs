using TruckingTmsMaui.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TruckingTmsMaui.Core.Entities
{
    // Core Job/Load (Order) Entity - Includes all required fields (1.A-D)
    public class Job
    {
        [Key]
        public int Id { get; set; }
        public JobStatus Status { get; set; } = JobStatus.Draft;

        // A. Basic Job & Load Details
        public string InvoiceNumber { get; set; } = string.Empty;
        public string TicketNumber { get; set; } = string.Empty;
        public string JobName { get; set; } = string.Empty;
        public string JobNumber { get; set; } = string.Empty;
        public string CommodityBeingHauled { get; set; } = string.Empty;
        
        public string PickupAddress { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public double TotalMiles { get; set; }
        public decimal RatePerLoad { get; set; }
        
        // TotalRevenue (auto-calculated property for Leaser jobs, backed by the field)
        public decimal TotalRevenue { get; set; } 
        
        public string BolNumber { get; set; } = string.Empty;
        public string NotesSpecialInstructions { get; set; } = string.Empty;

        // Date fields
        public DateTime JobDate { get; set; } = DateTime.Today;
        public DateTime PickupDate { get; set; } = DateTime.Today.AddDays(1);
        public DateTime DeliveryDate { get; set; } = DateTime.Today.AddDays(1);
        public DateTime? InvoiceDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // B. Driver Information
        public int? DriverId { get; set; }
        [ForeignKey(nameof(DriverId))]
        public virtual Driver? Driver { get; set; }
        
        public string DriverPhoneNumber { get; set; } = string.Empty; // Cached snapshot
        public string TruckNumber { get; set; } = string.Empty;
        public string TrailerNumber { get; set; } = string.Empty;
        
        public bool IsDispatched { get; set; }
        public DateTime? DispatchDateTime { get; set; }
        
        public DateTime? EstimatedPickupTime { get; set; }
        public DateTime? EstimatedDeliveryTime { get; set; }
        public DateTime? ActualPickupTime { get; set; }
        public DateTime? ActualDeliveryTime { get; set; }

        // C. Customer & Broker Info
        public int? CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public virtual Customer? Customer { get; set; }
        
        public int? ClientProfileId { get; set; }
        [ForeignKey(nameof(ClientProfileId))]
        public virtual ClientProfile? ClientProfile { get; set; }

        public string BrokerOrCustomerName { get; set; } = string.Empty; // Snapshot string
        public string BrokerContactName { get; set; } = string.Empty;
        public string BrokerPhoneNumber { get; set; } = string.Empty;
        public string BrokerEmail { get; set; } = string.Empty;
        
        public bool RateConfirmationUploaded { get; set; }
        public BillingTerms CustomerBillingTerms { get; set; } = BillingTerms.Net30;
        
        // D. Leaser / Outsourced Work Section
        public bool IsLeaser { get; set; }

        public string LeaserCompanyName { get; set; } = string.Empty;
        public string LeaserDriverName { get; set; } = string.Empty;
        public string LeaserInvoiceNumber { get; set; } = string.Empty;
        
        public decimal RateLeaserChargesYou { get; set; }
        public decimal RateYouChargeClient { get; set; }

        [NotMapped]
        public decimal ProfitMargin => IsLeaser ? RateYouChargeClient - RateLeaserChargesYou : 0m;
        
        public bool LeaserBillEnteredInQbo { get; set; }
        
        // E. Document Attachments
        public virtual ICollection<DocumentAttachment> DocumentAttachments { get; set; } = new List<DocumentAttachment>();
    }
}