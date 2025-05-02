using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaPortal.Core.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        IBaseRepo<T> GetRepository<T>() where T : class;
        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
}
