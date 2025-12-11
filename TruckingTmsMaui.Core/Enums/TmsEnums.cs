namespace TruckingTmsMaui.Core.Enums
{
    // 3. AUTH, ROLES, AND SECURITY
    public enum UserRole
    {
        Admin,
        Dispatcher,
        FleetOps,
        Finance,
        Driver
    }

    // 1.A - Job/Load Status
    public enum JobStatus
    {
        Draft,
        Scheduled,
        InTransit,
        Delivered,
        Cancelled,
        Invoiced,
        Paid
    }

    // 1.C - Customer Billing Terms (Easily extendable)
    public enum BillingTerms
    {
        Net7,
        Net14,
        Net30,
        Net60,
        COD // Cash On Delivery
    }

    // 1.E - Document Type
    public enum DocumentType
    {
        Bol, // Bill of Lading
        RateConfirmation,
        Pod, // Proof of Delivery
        Ticket,
        LeaserInvoice,
        CustomerInvoice,
        Other
    }

    // 2.Configuration
    public enum DistanceUnit
    {
        Miles,
        Kilometers
    }

    public enum CurrencyUnit
    {
        USD
    }
}