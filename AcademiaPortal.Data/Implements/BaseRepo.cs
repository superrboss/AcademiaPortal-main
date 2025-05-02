using AcademiaPortal.Core.Interfaces;
using AcademiaPortal.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaPortal.Data.Implements
{
    public class BaseRepo<T> : IBaseRepo<T> where T : class 
    {
        private readonly AppDbContext _context;
        public BaseRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<T>  Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<T> Delete(int id)
        {
            T entity = await _context.Set<T>().FindAsync(id);
           _context.Set<T>().Remove(entity);
            return entity;
        }

        public async Task<T> Edit(T entity)
        {
            
            _context.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        
    }

        public async Task<IEnumerable<T>> FindAllWhen(System.Linq.Expressions.Expression<Func<T, bool>> Exp)
        {
            return await _context.Set<T>().Where(Exp).ToListAsync();

        }
        public async Task<T> FindWhen(System.Linq.Expressions.Expression<Func<T, bool>> Exp)
        {
            return await _context.Set<T>().Where(Exp).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByID(int id)
        {
           return await _context.Set<T>().FindAsync(id);   
        }
        public async Task<IEnumerable<T>> GetAllIncluding(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

    }

}
