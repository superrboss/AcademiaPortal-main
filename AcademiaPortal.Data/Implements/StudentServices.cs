using AcademiaPortal.Core.Interfaces;
using AcademiaPortal.Core.Models;
using AcademiaPortal.Core.Options;
using Microsoft.Extensions.Options;
using AcademiaPortal.Data.UserServies;
using System.IdentityModel.Tokens.Jwt;
using AcademiaPortal.Core.Services;
namespace AcademiaPortal.Data.Implements
{
    public class StudentServices : IStudentServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOptions<JwtMap> _jwt;
        public StudentServices(IUnitOfWork unitOfWork, IOptions<JwtMap> jwt)
        {
            _unitOfWork = unitOfWork;
            _jwt = jwt;
        }
        public async Task<bool> RegisterStudent(Student NewStudent)
        {
            var GetRepo = _unitOfWork.GetRepository<Student>();
            await GetRepo.Add(NewStudent);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }
        public async Task<IEnumerable<Student>> GetAllStudents()
        {
            var GetRepo = _unitOfWork.GetRepository<Student>();
            return await GetRepo.GetAll();
        }
        public async Task<bool> DeleteStudent(int studentId)
        {
            var GetRepo = _unitOfWork.GetRepository<Student>();
            GetRepo.Delete(studentId);
            var resukt = await _unitOfWork.SaveChangesAsync();
            return resukt > 0;
        }
        public async Task<Student> GetStudentByEmail(string email)
        {
            var GetRepo = _unitOfWork.GetRepository<Student>();
            return await GetRepo.FindWhen(s => s.Email == email);
        }
        public async Task<Student> GetStudentById(int studentId)
        {
            var GetRepo = _unitOfWork.GetRepository<Student>();
            var student = await GetRepo.GetByID(studentId);
            if (student == null)
                return null;
            return student;
        }
        public async Task<bool> UpdateStudent(Student UpdatedStudent)
        {
            var getRepo = _unitOfWork.GetRepository<Student>();
            await getRepo.Edit(UpdatedStudent);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }
        public async Task<string> LoginStudent(string email, string password)
        {
         Student SerachAboutStudent= await GetStudentByEmail(email);
            if (SerachAboutStudent == null)
                return "NotFound this SerachAboutStudent";

            if (!HelpPassword.Verify(password, SerachAboutStudent.Password))
                return "Error Password";

            TokenServices tokenService = new TokenServices(_jwt);
            var token = tokenService.GenerateToken(SerachAboutStudent);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<IEnumerable<Student>> GetStudentByName(string Name)
        {
            var GetRepo = _unitOfWork.GetRepository<Student>();
            return await GetRepo.FindAllWhen(s => s.FullName == Name);
        }
        public async Task<bool> RegisterSubject(StudentSubject student_subject)
        {
            var GetRepo = _unitOfWork.GetRepository<StudentSubject>();
            await GetRepo.Add(student_subject);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }
        public async Task<IEnumerable<string>> GetAllMyDoctor(int My_ID)
        {
            var getRepo = _unitOfWork.GetRepository<StudentSubject>();
            var subjectRepo = _unitOfWork.GetRepository<Subject>();
            List<string> doctorNames = new List<string>();

            var studentSubjects = await getRepo.FindAllWhen(s => s.StudentId == My_ID);
            var subjectIds = studentSubjects.Select(s => s.SubjectId).ToList();

            // Load subjects including their doctors
            var subjectsWithDoctors = await subjectRepo.GetAllIncluding(s => s.Doctor);

            foreach (var subject in subjectsWithDoctors.Where(s => subjectIds.Contains(s.Id)))
            {
                if (subject.Doctor != null)
                {
                    doctorNames.Add(subject.Doctor.FullName);
                }
            }

            return doctorNames;

        }
    }
}


