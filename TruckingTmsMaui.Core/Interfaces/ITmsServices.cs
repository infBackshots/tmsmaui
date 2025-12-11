using TruckingTmsMaui.Core.Entities;
using TruckingTmsMaui.Core.Enums;

namespace TruckingTmsMaui.Core.Interfaces
{
    // Base interface for all services
    public interface IDataService<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }

    // Specialized services
    public interface IJobService : IDataService<Job>
    {
        Task<IEnumerable<Job>> GetJobsByStatusAsync(JobStatus status);
        Task<IEnumerable<Job>> GetLeaserJobsAsync();
    }

    public interface IDriverService : IDataService<Driver>
    {
        Task<IEnumerable<Driver>> GetAvailableDriversAsync();
    }
    
    public interface IAuthService
    {
        Task<User?> LoginAsync(string username, string password);
        User? GetCurrentUser();
        UserRole GetCurrentUserRole();
        bool IsUserInRole(UserRole role);
    }

    public interface IDocumentService
    {
        // Platform-specific file picking and storage logic would go here.
        Task<DocumentAttachment?> AttachDocumentToJobAsync(int jobId, DocumentType type, string filePath);
        Task<IEnumerable<DocumentAttachment>> GetDocumentsByJobIdAsync(int jobId);
    }

    public interface ISeedingService
    {
        Task SeedDatabaseAsync();
    }
}