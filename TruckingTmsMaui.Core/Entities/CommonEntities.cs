using TruckingTmsMaui.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace TruckingTmsMaui.Core.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string HashedPassword { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Driver;
        public DateTime LastLogin { get; set; } = DateTime.MinValue;
    }

    // 1.B Driver Information
    public class Driver
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;

        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }

    // 1.C Customer & Broker Info
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string BillingAddress { get; set; } = string.Empty;
        public BillingTerms DefaultBillingTerms { get; set; } = BillingTerms.Net30;
        public virtual ICollection<ClientProfile> ClientProfiles { get; set; } = new List<ClientProfile>();
    }

    public class Vehicle
    {
        [Key]
        public int Id { get; set; }
        public string TruckNumber { get; set; } = string.Empty;
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public double CurrentOdometer { get; set; }
        public bool IsInService { get; set; } = true;
    }
    
    // Simple Trip entity for Dispatch and Driver mobile views
    public class Trip 
    {
        [Key]
        public int Id { get; set; }
        public int DriverId { get; set; }
        public virtual Driver? Driver { get; set; }
        public string TruckNumber { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Status { get; set; } = "Scheduled"; // e.g., Scheduled, Started, Completed

        // Link to jobs in this trip (many-to-many relationship simplified to one-to-many for MVP)
        public int? JobId { get; set; } 
        public virtual Job? Job { get; set; }
    }
}