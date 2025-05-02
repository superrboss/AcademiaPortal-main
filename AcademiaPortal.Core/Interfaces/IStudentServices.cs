using AcademiaPortal.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaPortal.Core.Interfaces
{
    public interface IStudentServices
    {
        Task<bool> RegisterStudent(Student NewStudent);
        Task<string> LoginStudent(string email, string password);
        Task<bool> UpdateStudent(Student UpdatedStudent);
        Task<bool> DeleteStudent(int studentId);
        Task<IEnumerable<Student>> GetAllStudents();
        Task<Student> GetStudentById(int studentId);
        Task<Student> GetStudentByEmail(string email);
        Task<IEnumerable<Student>> GetStudentByName(string Name);
        Task<bool> RegisterSubject(StudentSubject student_subject);
        public Task<IEnumerable<string>> GetAllMyDoctor(int My_ID);


    }
}
