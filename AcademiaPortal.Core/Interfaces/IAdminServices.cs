using AcademiaPortal.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaPortal.Core.Interfaces
{
    public interface IAdminServices
    {
        Task<string> LoginAdmin(string email, string password);
        public Task<Admin> GetAdminByEmail(string emaul);

    }
}
