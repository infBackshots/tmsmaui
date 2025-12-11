using Microsoft.EntityFrameworkCore;
using TruckingTmsMaui.Core.Entities;
using TruckingTmsMaui.Core.Enums;
using TruckingTmsMaui.Core.Interfaces;
using TruckingTmsMaui.Data.Context;
using System.Security.Cryptography;
using System.Text;

namespace TruckingTmsMaui.Data.Repositories
{
    // Utility for basic password hashing (for demo purposes)
    public static class AuthHelper
    {
        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }

    // Concrete implementation of IAuthService
    public class AuthService : IAuthService
    {
        private readonly TruckingTmsDbContext _context;
        private User? _currentUser;

        public AuthService(TruckingTmsDbContext context)
        {
            _context = context;
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user != null && user.HashedPassword == AuthHelper.HashPassword(password))
            {
                user.LastLogin = DateTime.UtcNow;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                _currentUser = user;
                return user;
            }
            return null;
        }

        public User? GetCurrentUser() => _currentUser;
        
        public UserRole GetCurrentUserRole() => _currentUser?.Role ?? UserRole.Driver;

        public bool IsUserInRole(UserRole role)
        {
            // Admin role can access anything
            if (_currentUser?.Role == UserRole.Admin) return true;
            return _currentUser?.Role == role;
        }
    }

    // Concrete implementation of ISeedingService
    public class SeedingService : ISeedingService
    {
        private readonly TruckingTmsDbContext _context;

        public SeedingService(TruckingTmsDbContext context)
        {
            _context = context;
        }
        
        public async Task SeedDatabaseAsync()
        {
            // Apply migrations and seed data
            await _context.Database.EnsureCreatedAsync();

            if (!await _context.Users.AnyAsync())
            {
                await TmsDataSeeder.SeedUsers(_context);
            }
            if (!await _context.Customers.AnyAsync())
            {
                await TmsDataSeeder.SeedAllData(_context);
            }
        }
    }

    // Concrete Document Service (Stubs file management logic)
    public class DocumentService : IDocumentService
    {
        private readonly TruckingTmsDbContext _context;

        public DocumentService(TruckingTmsDbContext context)
        {
            _context = context;
        }

        // Simulates file picking and attachment
        public async Task<DocumentAttachment?> AttachDocumentToJobAsync(int jobId, DocumentType type, string filePath)
        {
            // In a real app, this would handle copying the file to a secure location (e.g., cloud storage/local app data)
            // and generating a unique ID/path. For this MAUI stub, we use the provided path and mock the upload.

            var job = await _context.Jobs.FindAsync(jobId);
            var user = DependencyService.Get<IAuthService>()?.GetCurrentUser();

            if (job == null || user == null) return null;

            var attachment = new DocumentAttachment
            {
                JobId = jobId,
                DocumentType = type,
                FileName = Path.GetFileName(filePath),
                FilePathOrBlobId = filePath, // Stubbed file path/identifier
                UploadedAt = DateTime.UtcNow,
                UploadedByUserId = user.Id
            };

            await _context.DocumentAttachments.AddAsync(attachment);
            await _context.SaveChangesAsync();
            return attachment;
        }

        public async Task<IEnumerable<DocumentAttachment>> GetDocumentsByJobIdAsync(int jobId)
        {
            return await _context.DocumentAttachments
                .Where(da => da.JobId == jobId)
                .ToListAsync();
        }
    }

    // Other services (Driver, Vehicle, Customer, ClientProfile)
    public class DriverService : IDataService<Driver>, IDriverService
    {
        private readonly TruckingTmsDbContext _context;
        public DriverService(TruckingTmsDbContext context) => _context = context;
        public async Task<Driver?> GetByIdAsync(int id) => await _context.Drivers.FindAsync(id);
        public async Task<IEnumerable<Driver>> GetAllAsync() => await _context.Drivers.ToListAsync();
        public async Task<Driver> AddAsync(Driver entity) { _context.Drivers.Add(entity); await _context.SaveChangesAsync(); return entity; }
        public async Task UpdateAsync(Driver entity) { _context.Drivers.Update(entity); await _context.SaveChangesAsync(); }
        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Drivers.FindAsync(id);
            if (entity != null) { _context.Drivers.Remove(entity); await _context.SaveChangesAsync(); }
        }
        public async Task<IEnumerable<Driver>> GetAvailableDriversAsync() => await _context.Drivers.Where(d => d.IsAvailable).ToListAsync();
    }
    
    // Other common services (Stubbed for brevity, following IDataService pattern)
    public class VehicleService : IDataService<Vehicle>
    {
        private readonly TruckingTmsDbContext _context;
        public VehicleService(TruckingTmsDbContext context) => _context = context;
        public async Task<Vehicle?> GetByIdAsync(int id) => await _context.Vehicles.FindAsync(id);
        public async Task<IEnumerable<Vehicle>> GetAllAsync() => await _context.Vehicles.ToListAsync();
        public async Task<Vehicle> AddAsync(Vehicle entity) { _context.Vehicles.Add(entity); await _context.SaveChangesAsync(); return entity; }
        public async Task UpdateAsync(Vehicle entity) { _context.Vehicles.Update(entity); await _context.SaveChangesAsync(); }
        public async Task DeleteAsync(int id) { var entity = await _context.Vehicles.FindAsync(id); if (entity != null) { _context.Vehicles.Remove(entity); await _context.SaveChangesAsync(); } }
    }
    
    public class CustomerService : IDataService<Customer>
    {
        private readonly TruckingTmsDbContext _context;
        public CustomerService(TruckingTmsDbContext context) => _context = context;
        public async Task<Customer?> GetByIdAsync(int id) => await _context.Customers.FindAsync(id);
        public async Task<IEnumerable<Customer>> GetAllAsync() => await _context.Customers.ToListAsync();
        public async Task<Customer> AddAsync(Customer entity) { _context.Customers.Add(entity); await _context.SaveChangesAsync(); return entity; }
        public async Task UpdateAsync(Customer entity) { _context.Customers.Update(entity); await _context.SaveChangesAsync(); }
        public async Task DeleteAsync(int id) { var entity = await _context.Customers.FindAsync(id); if (entity != null) { _context.Customers.Remove(entity); await _context.SaveChangesAsync(); } }
    }
    
    public class ClientProfileService : IDataService<ClientProfile>
    {
        private readonly TruckingTmsDbContext _context;
        public ClientProfileService(TruckingTmsDbContext context) => _context = context;
        public async Task<ClientProfile?> GetByIdAsync(int id) => await _context.ClientProfiles.FindAsync(id);
        public async Task<IEnumerable<ClientProfile>> GetAllAsync() => await _context.ClientProfiles.ToListAsync();
        public async Task<ClientProfile> AddAsync(ClientProfile entity) { _context.ClientProfiles.Add(entity); await _context.SaveChangesAsync(); return entity; }
        public async Task UpdateAsync(ClientProfile entity) { _context.ClientProfiles.Update(entity); await _context.SaveChangesAsync(); }
        public async Task DeleteAsync(int id) { var entity = await _context.ClientProfiles.FindAsync(id); if (entity != null) { _context.ClientProfiles.Remove(entity); await _context.SaveChangesAsync(); } }
    }
}