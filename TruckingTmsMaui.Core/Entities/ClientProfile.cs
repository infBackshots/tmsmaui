using TruckingTmsMaui.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace TruckingTmsMaui.Core.Entities
{
    // 6) Client Profiles
    public class ClientProfile
    {
        public int Id { get; set; }
        public string DisplayName { get; set; } = string.Empty; // e.g., "Walmart SLC DC – Shipping"
        public int CustomerId { get; set; }
        public virtual Customer? Customer { get; set; } // FK to Customer

        // PaperworkDestination
        public string PaperworkDestinationAddress { get; set; } = string.Empty;
        public string PaperworkDestinationEmail { get; set; } = string.Empty;

        // Preferred Contact
        public string PreferredContactName { get; set; } = string.Empty;
        public string PreferredContactPhone { get; set; } = string.Empty;
        public string PreferredContactEmail { get; set; } = string.Empty;
        
        // FrequentDrivers (Stored as JSON or comma-separated IDs in a real DB, but simplified here)
        public string FrequentDriverIds { get; set; } = string.Empty; 

        // PreferredEquipment
        public string PreferredEquipmentType { get; set; } = string.Empty; // e.g., "Dry Van"

        // ServiceInstructions
        public string ServiceInstructions { get; set; } = string.Empty; // "Use dock 4", "Appointment required"
        
        // AccessHours (Simplified)
        public string AccessHours { get; set; } = string.Empty; 

        public string Tags { get; set; } = string.Empty; // E.g., "High priority"
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}