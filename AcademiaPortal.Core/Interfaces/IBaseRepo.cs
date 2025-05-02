using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaPortal.Core.Interfaces
{
    public interface IBaseRepo<T> where T : class
    {
        Task<T> Add(T entity);
        Task<T> Edit(T entity);
        Task<T> GetByID(int id);
        Task<IEnumerable<T>> GetAll();
        Task<T> Delete(int id);
        Task<T> FindWhen(Expression<Func<T, bool>> Exp);
        Task<IEnumerable<T>> FindAllWhen(Expression<Func<T, bool>> Exp);
        Task<IEnumerable<T>> GetAllIncluding(params Expression<Func<T, object>>[] includes);

    }
}
