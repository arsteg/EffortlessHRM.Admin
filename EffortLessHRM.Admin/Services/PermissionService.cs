using EffortLessHRM.Admin.Data;
using EffortLessHRM.Admin.Models;
using EffortLessHRM.Admin.Utility;
using MongoDB.Driver;

namespace EffortLessHRM.Admin.Services
{
    public class PermissionService
    {
        private readonly MongoDbContext _dbContext;

        public PermissionService(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Permission>> GetAllAsync()
        {
            return await _dbContext.PermissionCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Permission?> GetByIdAsync(string id)
        {
            return await _dbContext.PermissionCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Permission?> GetByNameAsync(string permissionName)
        {
            return await _dbContext.PermissionCollection
                .Find(p => p.PermissionName.ToLower() == permissionName.ToLower())
                .FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Permission permission)
        {
            if (permission == null)
                throw new ArgumentNullException(nameof(permission));

            var exists = await GetByNameAsync(permission.PermissionName);
            if (exists != null)
                throw new InvalidOperationException("Permission already exists.");

            await _dbContext.PermissionCollection.InsertOneAsync(permission);
        }

        public async Task UpdateAsync(string id, Permission updatedPermission)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            var existing = await GetByIdAsync(id);
            if (existing == null)
                throw new InvalidOperationException("Permission not found.");

            updatedPermission.Id = id; // Ensure ID is preserved
            await _dbContext.PermissionCollection.ReplaceOneAsync(p => p.Id == id, updatedPermission);
        }

        public async Task DeleteAsync(string id)
        {
            await _dbContext.PermissionCollection.DeleteOneAsync(p => p.Id == id);
        }
    }
}
