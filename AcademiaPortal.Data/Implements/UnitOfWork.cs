using AcademiaPortal.Core.Interfaces;
using AcademiaPortal.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaPortal.Data.Implements
{
    public class UnitOfWork : IUnitOfWork
    {
        // The shared DbContext instance for all operations
        private readonly AppDbContext _context;

        // This dictionary holds created repositories (so we don't create new ones every time)
        private readonly Dictionary<Type, object> _repositories = new();

        // Constructor takes DbContext (your AppDbContext) via Dependency Injection
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        // Generic repository getter: returns the repository for type T (like Student, Doctor, etc.)
        public IBaseRepo<T> GetRepository<T>() where T : class
        {
            var type = typeof(T); // Get the actual class type of T

            // If we haven't created a repo for this type before, create and store it
            if (!_repositories.ContainsKey(type))
            {
                var repoInstance = new BaseRepo<T>(_context); // Create repo for T
                _repositories[type] = repoInstance;           // Store in dictionary
            }

            // Return the repo from the dictionary (cast back to correct type)
            return (IBaseRepo<T>)_repositories[type];
        }

        // Save changes to the database asynchronously
        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        // Save changes synchronously (if you don't want async)
        public int SaveChanges() => _context.SaveChanges();

        // Dispose the DbContext when done
        public void Dispose() => _context.Dispose();
    }

}
