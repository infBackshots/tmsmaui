using TruckingTmsMaui.Core.Entities;
using TruckingTmsMaui.Core.Enums;
using TruckingTmsMaui.Data.Context;
using TruckingTmsMaui.Data.Repositories;

namespace TruckingTmsMaui.Data.Seeding
{
    public static class TmsDataSeeder
    {
        private static IEnumerable<User> GetSeedUsers()
        {
            // Passwords are all "P@ssw0rd1"
            var hashedPassword = AuthHelper.HashPassword("P@ssw0rd1");

            return new List<User>
            {
                new User { Id = 1, Username = "admin", HashedPassword = hashedPassword, FullName = "System Administrator", Role = UserRole.Admin },
                new User { Id = 2, Username = "dispatch", HashedPassword = hashedPassword, FullName = "Sarah Dispatch", Role = UserRole.Dispatcher },
                new User { Id = 3, Username = "fleetops", HashedPassword = hashedPassword, FullName = "Mark Fleet", Role = UserRole.FleetOps },
                new User { Id = 4, Username = "finance", HashedPassword = hashedPassword, FullName = "Jessica Finance", Role = UserRole.Finance },
                new User { Id = 5, Username = "driver1", HashedPassword = hashedPassword, FullName = "Joe Driver", Role = UserRole.Driver },
                new User { Id = 6, Username = "driver2", HashedPassword = hashedPassword, FullName = "Jane Driver", Role = UserRole.Driver }
            };
        }

        private static IEnumerable<Driver> GetSeedDrivers() => new List<Driver>
        {
            new Driver { Id = 101, FirstName = "Joe", LastName = "Driver", PhoneNumber = "801-555-0101", EmployeeId = "D101", IsAvailable = true },
            new Driver { Id = 102, FirstName = "Jane", LastName = "Driver", PhoneNumber = "801-555-0102", EmployeeId = "D102", IsAvailable = true },
            new Driver { Id = 103, FirstName = "Leaser", LastName = "Max", PhoneNumber = "801-555-0103", EmployeeId = "L001", IsAvailable = false }
        };
        
        private static IEnumerable<Vehicle> GetSeedVehicles() => new List<Vehicle>
        {
            new Vehicle { Id = 1, TruckNumber = "T101", Make = "Freightliner", Model = "Cascadia", Year = 2020, CurrentOdometer = 150000, IsInService = true },
            new Vehicle { Id = 2, TruckNumber = "T102", Make = "Volvo", Model = "VNL", Year = 2018, CurrentOdometer = 210000, IsInService = false },
            new Vehicle { Id = 3, TruckNumber = "T103", Make = "Kenworth", Model = "T680", Year = 2022, CurrentOdometer = 55000, IsInService = true }
        };
        
        private static IEnumerable<Customer> GetSeedCustomers() => new List<Customer>
        {
            new Customer { Id = 1, Name = "Utah Distribution Center Inc.", BillingAddress = "123 S Redwood Rd, Salt Lake City, UT 84104", DefaultBillingTerms = BillingTerms.Net30 },
            new Customer { Id = 2, Name = "Ogden Manufacturing Co.", BillingAddress = "45 E 12th St, Ogden, UT 84404", DefaultBillingTerms = BillingTerms.Net14 },
            new Customer { Id = 3, Name = "Provo Logistics Brokerage", BillingAddress = "78 N University Ave, Provo, UT 84601", DefaultBillingTerms = BillingTerms.Net7 }
        };
        
        private static IEnumerable<ClientProfile> GetSeedClientProfiles(List<Customer> customers)
        {
            var utahDC = customers.First(c => c.Name.Contains("Utah Distribution"));
            var ogdenMfg = customers.First(c => c.Name.Contains("Ogden Manufacturing"));
            
            return new List<ClientProfile>
            {
                new ClientProfile 
                { 
                    Id = 1, 
                    CustomerId = utahDC.Id, 
                    DisplayName = "SLC Dock 5 Receiving",
                    PaperworkDestinationEmail = "slcreceiving@utahdc.com",
                    ServiceInstructions = "Enter via gate 3. Must have hardhat. Pallet exchange required.",
                    AccessHours = "Mon-Fri 08:00 - 16:00 MST",
                    Tags = "High priority, Slow unloading"
                },
                new ClientProfile 
                { 
                    Id = 2, 
                    CustomerId = ogdenMfg.Id, 
                    DisplayName = "Ogden Warehouse Shipping",
                    PaperworkDestinationEmail = "shipping@ogdenmfg.com",
                    ServiceInstructions = "Call 30 minutes before arrival. Use dock 1. Appointment required.",
                    AccessHours = "Mon-Sun 24/7",
                    Tags = "Appointment required"
                }
            };
        }

        private static IEnumerable<Job> GetSeedJobs(List<Driver> drivers, List<Customer> customers, List<ClientProfile> profiles)
        {
            var driver1 = drivers.First(d => d.EmployeeId == "D101");
            var driver2 = drivers.First(d => d.EmployeeId == "D102");
            var customer1 = customers.First(c => c.Name.Contains("Utah Distribution"));
            var profile1 = profiles.First(p => p.DisplayName.Contains("SLC Dock"));
            
            return new List<Job>
            {
                // 1. Regular Job (In Transit)
                new Job
                {
                    Id = 1001,
                    Status = JobStatus.InTransit,
                    InvoiceNumber = "INV-1001",
                    JobNumber = "UT-1001",
                    JobName = "SLC to Provo Hot Shot",
                    CommodityBeingHauled = "Automotive Parts",
                    PickupAddress = "1500 N 2200 W, Salt Lake City, UT 84116",
                    DeliveryAddress = "420 W 1500 S, Provo, UT 84601",
                    TotalMiles = 95.5,
                    RatePerLoad = 650.00m,
                    TotalRevenue = 650.00m,
                    BolNumber = "BOL-A1234",
                    DriverId = driver1.Id,
                    DriverPhoneNumber = driver1.PhoneNumber,
                    TruckNumber = "T101",
                    TrailerNumber = "TR301",
                    IsDispatched = true,
                    DispatchDateTime = DateTime.Now.AddHours(-3),
                    EstimatedPickupTime = DateTime.Now.AddHours(-2),
                    EstimatedDeliveryTime = DateTime.Now.AddHours(2),
                    ActualPickupTime = DateTime.Now.AddHours(-1),
                    CustomerBillingTerms = BillingTerms.Net30,
                    BrokerOrCustomerName = customer1.Name,
                    CustomerId = customer1.Id,
                    ClientProfileId = profile1.Id,
                    RateConfirmationUploaded = true,
                },
                // 2. Leaser Job (Delivered, Ready to Invoice)
                new Job
                {
                    Id = 1002,
                    Status = JobStatus.Delivered,
                    InvoiceNumber = "",
                    JobNumber = "UT-1002",
                    JobName = "West Valley Reefer Load",
                    CommodityBeingHauled = "Frozen Produce",
                    PickupAddress = "2800 S 3200 W, West Valley City, UT 84119",
                    DeliveryAddress = "500 N Main St, Orem, UT 84057",
                    TotalMiles = 50.2,
                    RatePerLoad = 0m, // Revenue calculated via Leaser fields
                    TotalRevenue = 800.00m,
                    BolNumber = "BOL-B5678",
                    TruckNumber = "Leaser-Truck",
                    TrailerNumber = "Reefer",
                    IsDispatched = true,
                    IsLeaser = true, // REQUIRED LEASER FLAG
                    LeaserCompanyName = "Max Transport Services",
                    LeaserDriverName = "Leaser Max",
                    LeaserInvoiceNumber = "LIV-4488",
                    RateLeaserChargesYou = 550.00m, // Your Cost
                    RateYouChargeClient = 800.00m, // Your Revenue
                    LeaserBillEnteredInQbo = false,
                    CustomerBillingTerms = BillingTerms.Net7,
                    BrokerOrCustomerName = customers.First(c => c.Name.Contains("Provo Logistics")).Name,
                    CustomerId = customers.First(c => c.Name.Contains("Provo Logistics")).Id,
                    RateConfirmationUploaded = true,
                    ActualDeliveryTime = DateTime.Now.AddDays(-1)
                },
                // 3. Draft Job (Unassigned)
                new Job
                {
                    Id = 1003,
                    Status = JobStatus.Draft,
                    JobNumber = "UT-1003",
                    JobName = "Ogden Rail Yard Pickup",
                    CommodityBeingHauled = "Steel Coils",
                    PickupAddress = "1200 S Depot Dr, Ogden, UT 84401",
                    DeliveryAddress = "300 E 400 S, Salt Lake City, UT 84111",
                    TotalMiles = 45.0,
                    RatePerLoad = 500.00m,
                    TotalRevenue = 500.00m,
                    CustomerBillingTerms = BillingTerms.Net30,
                    BrokerOrCustomerName = customers.First(c => c.Name.Contains("Ogden Manufacturing")).Name,
                    CustomerId = customers.First(c => c.Name.Contains("Ogden Manufacturing")).Id,
                    RateConfirmationUploaded = false,
                },
                // 4. Invoiced Job (Completed)
                new Job
                {
                    Id = 1004,
                    Status = JobStatus.Invoiced,
                    InvoiceNumber = "INV-0999",
                    JobNumber = "UT-0999",
                    JobName = "Midvale Dry Van",
                    CommodityBeingHauled = "Retail Goods",
                    PickupAddress = "7150 S 1000 E, Midvale, UT 84047",
                    DeliveryAddress = "350 S 400 W, Salt Lake City, UT 84101",
                    TotalMiles = 15.0,
                    RatePerLoad = 350.00m,
                    TotalRevenue = 350.00m,
                    BolNumber = "BOL-C9999",
                    DriverId = driver2.Id,
                    DriverPhoneNumber = driver2.PhoneNumber,
                    TruckNumber = "T103",
                    TrailerNumber = "TR303",
                    IsDispatched = true,
                    CustomerBillingTerms = BillingTerms.Net14,
                    BrokerOrCustomerName = customer1.Name,
                    CustomerId = customer1.Id,
                    ClientProfileId = null,
                    RateConfirmationUploaded = true,
                    InvoiceDate = DateTime.Now.AddDays(-5),
                    ActualDeliveryTime = DateTime.Now.AddDays(-7)
                }
            };
        }

        private static IEnumerable<DocumentAttachment> GetSeedDocumentAttachments(List<Job> jobs, List<User> users)
        {
            var job1 = jobs.First(j => j.JobNumber == "UT-1001");
            var job2 = jobs.First(j => j.JobNumber == "UT-1002");
            var job4 = jobs.First(j => j.JobNumber == "UT-0999");
            var admin = users.First(u => u.Role == UserRole.Admin);
            
            return new List<DocumentAttachment>
            {
                // Job 1001 Documents (In-Transit)
                new DocumentAttachment { Id = 1, JobId = job1.Id, DocumentType = DocumentType.RateConfirmation, FileName = "UT1001_RC.pdf", FilePathOrBlobId = "stub/ut1001/rc.pdf", UploadedByUserId = admin.Id },
                new DocumentAttachment { Id = 2, JobId = job1.Id, DocumentType = DocumentType.Bol, FileName = "UT1001_BOL.pdf", FilePathOrBlobId = "stub/ut1001/bol.pdf", UploadedByUserId = admin.Id },
                
                // Job 1002 Documents (Leaser)
                new DocumentAttachment { Id = 3, JobId = job2.Id, DocumentType = DocumentType.LeaserInvoice, FileName = "LIV-4488.pdf", FilePathOrBlobId = "stub/ut1002/leaser_inv.pdf", UploadedByUserId = admin.Id },
                new DocumentAttachment { Id = 4, JobId = job2.Id, DocumentType = DocumentType.Pod, FileName = "UT1002_POD_Leaser.jpg", FilePathOrBlobId = "stub/ut1002/pod.jpg", UploadedByUserId = admin.Id },
                
                // Job 0999 Documents (Invoiced)
                new DocumentAttachment { Id = 5, JobId = job4.Id, DocumentType = DocumentType.CustomerInvoice, FileName = "INV-0999.pdf", FilePathOrBlobId = "stub/ut0999/cust_inv.pdf", UploadedByUserId = admin.Id },
                new DocumentAttachment { Id = 6, JobId = job4.Id, DocumentType = DocumentType.Pod, FileName = "UT0999_POD.pdf", FilePathOrBlobId = "stub/ut0999/pod.pdf", UploadedByUserId = admin.Id }
            };
        }

        public static async Task SeedUsers(TruckingTmsDbContext context)
        {
            await context.Users.AddRangeAsync(GetSeedUsers());
            await context.SaveChangesAsync();
        }

        public static async Task SeedAllData(TruckingTmsDbContext context)
        {
            var drivers = GetSeedDrivers().ToList();
            await context.Drivers.AddRangeAsync(drivers);
            
            var vehicles = GetSeedVehicles().ToList();
            await context.Vehicles.AddRangeAsync(vehicles);
            
            var customers = GetSeedCustomers().ToList();
            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();

            var profiles = GetSeedClientProfiles(customers).ToList();
            await context.ClientProfiles.AddRangeAsync(profiles);
            await context.SaveChangesAsync();

            var users = await context.Users.ToListAsync();
            var jobs = GetSeedJobs(drivers, customers, profiles).ToList();
            await context.Jobs.AddRangeAsync(jobs);
            await context.SaveChangesAsync();
            
            var attachments = GetSeedDocumentAttachments(jobs, users).ToList();
            await context.DocumentAttachments.AddRangeAsync(attachments);

            await context.SaveChangesAsync();
        }
    }
}