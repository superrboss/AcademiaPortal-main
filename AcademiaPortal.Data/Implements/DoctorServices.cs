using AcademiaPortal.Core.DTO;
using AcademiaPortal.Core.Interfaces;
using AcademiaPortal.Core.Models;
using AcademiaPortal.Core.Options;
using AcademiaPortal.Core.Services;
using AcademiaPortal.Data.UserServies;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace AcademiaPortal.Data.Implements
{
    public class DoctorServices : IDoctorServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOptions<JwtMap> _jwt;

        public DoctorServices(IUnitOfWork unitOfWork, IOptions<JwtMap> jwt)
        {
            _unitOfWork = unitOfWork;
            _jwt = jwt;
        }
        public async Task<bool> AddDoctor(Doctor New_Doc)
        {
           
            var getRepo = _unitOfWork.GetRepository<Doctor>();
            await getRepo.Add(New_Doc); 
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteDocftorById(int id)
        {
            var getRepo = _unitOfWork.GetRepository<Doctor>();
            var doctor = await getRepo.GetByID(id);
            if (doctor == null)
                return false;
            
            await getRepo.Delete(id);
            var result = await _unitOfWork.SaveChangesAsync();
             return result > 0;

        }

        public async Task<bool> EditDoctor(Doctor NewDoc)
        {
            var getRepo = _unitOfWork.GetRepository<Doctor>();
            //var doctor = await getRepo.GetByID(NewDoc.Id);
            //if (doctor == null)
            //    return false;

            await getRepo.Edit(NewDoc);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

       
        public async Task<IEnumerable<GetAllDoctorsDTO>> GetAllDoctors()
        {
            var getRepo = _unitOfWork.GetRepository<Doctor>();
           IEnumerable<Doctor> allDoctors = await getRepo.GetAllIncluding(d => d.Subjects);

            return allDoctors.Select(d => new GetAllDoctorsDTO
            {
                Id = d.Id,
                FullName = d.FullName,
                Email = d.Email,
                Password = d.Password,
                Role = d.Role,
                Subjects = d.Subjects.Select(s => s.Name).ToList()
            });
        }

        public async Task<Doctor> GetDoctorByEmail(string email)
        {
            var getRepo = _unitOfWork.GetRepository<Doctor>();
            return await getRepo.FindWhen(x => x.Email == email);
        }

        public async Task<Doctor> GetDoctorById(int id)
        {
            var getRepo = _unitOfWork.GetRepository<Doctor>();
            return await getRepo.GetByID(id);
        }

        public async Task<Doctor> GetDoctorByName(string name)
        {
            var getRepo = _unitOfWork.GetRepository<Doctor>();
            return await getRepo.FindWhen(x => x.FullName == name);
        }

        public async Task<string> LoginDoctor(string email, string password)
        {
            Doctor SerachAboutDoctor = await GetDoctorByEmail(email);
            if (SerachAboutDoctor == null)
                return "NotFound this SerachAboutDoctor";
           
            if (!HelpPassword.Verify(password, SerachAboutDoctor.Password))
                return "Error Password";

            TokenServices tokenService = new TokenServices(_jwt);
            var token = tokenService.GenerateToken(SerachAboutDoctor);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IEnumerable<string>> GetAllMyStudents(int doctorId)
        {
            var studentSubjectRepo = _unitOfWork.GetRepository<StudentSubject>();
            var subjectRepo = _unitOfWork.GetRepository<Subject>();
            var studentRepo = _unitOfWork.GetRepository<Student>();

            // Step 1: Get all Subject IDs for this doctor
            var doctorSubjects = await subjectRepo.FindAllWhen(s => s.DoctorId == doctorId);
            var doctorSubjectIds = doctorSubjects.Select(s => s.Id).ToList();

            // Step 2: Get all StudentSubjects where SubjectId matches Doctor's subjects
            var studentSubjects = await studentSubjectRepo.FindAllWhen(ss => doctorSubjectIds.Contains(ss.SubjectId));
            var studentIds = studentSubjects.Select(ss => ss.StudentId).Distinct().ToList();

            // Step 3: Get all Students
            var students = await studentRepo.FindAllWhen(st => studentIds.Contains(st.Id));

            // Return student names (you can adjust based on your `Student` class)
            return students.Select(st => st.FullName);  // Or st.FullName, st.Email, etc.
        }


    }
}
