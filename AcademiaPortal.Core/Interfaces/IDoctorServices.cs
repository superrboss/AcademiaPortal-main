using AcademiaPortal.Core.DTO;
using AcademiaPortal.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaPortal.Core.Interfaces
{
    public interface IDoctorServices
    {
        public Task<bool> AddDoctor(Doctor New_Doc);
        Task<string> LoginDoctor(string email, string password);
        public Task<Doctor> GetDoctorById(int id);
        public Task<IEnumerable<GetAllDoctorsDTO>> GetAllDoctors();
        public Task<bool> DeleteDocftorById(int id);
        public Task<bool> EditDoctor(Doctor NewDoc);
        public Task<Doctor> GetDoctorByName(string name);
        public Task<Doctor> GetDoctorByEmail(string emaul);
        public Task<IEnumerable<string>> GetAllMyStudents(int doctorId);
    }
}
