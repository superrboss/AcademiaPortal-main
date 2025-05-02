using AcademiaPortal.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaPortal.Core.Interfaces
{
    public interface ISubjectServices
    {
        public Task<bool> AddSubject(Subject New_Subject);
        public Task<Subject> GetSubjectById(int id);
        public Task<IEnumerable<Subject>> GetAllSubjects();
        public Task<bool> DeleteSubjectById(int id);
        public Task<Subject> GetSubjectByName(string name);
        public Task<Subject> GetSubjectByDoctorId(int id);
        public Task<IEnumerable<Subject>> GetSubjectByDoctorName(string Name);
        public Task<IEnumerable<string>> GetAllMySubject(int My_ID);

    }
}
