using Microsoft.EntityFrameworkCore;
using TruckingTmsMaui.Core.Entities;
using TruckingTmsMaui.Core.Enums;
using TruckingTmsMaui.Core.Interfaces;
using TruckingTmsMaui.Data.Context;

namespace TruckingTmsMaui.Data.Repositories
{
    // Concrete implementation of IJobService using EF Core
    public class JobService : IJobService
    {
        private readonly TruckingTmsDbContext _context;

        public JobService(TruckingTmsDbContext context)
        {
            _context = context;
        }

        public async Task<Job?> GetByIdAsync(int id)
        {
            // Include related entities for a complete detail view
            return await _context.Jobs
                .Include(j => j.Driver)
                .Include(j => j.Customer)
                .Include(j => j.ClientProfile)
                .Include(j => j.DocumentAttachments)
                .FirstOrDefaultAsync(j => j.Id == id);
        }

        public async Task<IEnumerable<Job>> GetAllAsync()
        {
            return await _context.Jobs
                .Include(j => j.Customer)
                .ToListAsync();
        }

        public async Task<Job> AddAsync(Job entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Jobs.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Job entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Jobs.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job != null)
            {
                _context.Jobs.Remove(job);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Job>> GetJobsByStatusAsync(JobStatus status)
        {
            return await _context.Jobs
                .Where(j => j.Status == status)
                .Include(j => j.Customer)
                .ToListAsync();
        }

        public async Task<IEnumerable<Job>> GetLeaserJobsAsync()
        {
            return await _context.Jobs
                .Where(j => j.IsLeaser)
                .Include(j => j.Customer)
                .ToListAsync();
        }
    }
}