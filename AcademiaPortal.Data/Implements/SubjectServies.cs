using AcademiaPortal.Core.Interfaces;
using AcademiaPortal.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaPortal.Data.Implements
{
    public class SubjectServies : ISubjectServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public SubjectServies(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> AddSubject(Subject New_Subject)
        {
            var getRepo = _unitOfWork.GetRepository<Subject>();
            await getRepo.Add(New_Subject);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteSubjectById(int id)
        {
            var getRepo = _unitOfWork.GetRepository<Subject>();
            await getRepo.Delete(id);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<string>> GetAllMySubject(int My_ID)
        {
            var getRepo = _unitOfWork.GetRepository<StudentSubject>();
            var SubjectName = _unitOfWork.GetRepository<Subject>();
            List<String> mySubjects = new List<String>();

            var subjects = await getRepo.FindAllWhen(s => s.StudentId == My_ID);

            foreach (var subject in subjects)
            {
                var subjectDetails = await SubjectName.GetByID(subject.SubjectId);
                if (subjectDetails != null)
                {
                    mySubjects.Add(subjectDetails.Name);
                }
            }

            return mySubjects;
        }

        public async Task<IEnumerable<Subject>> GetAllSubjects()
        {
            //var getRepo = _unitOfWork.GetRepository<Subject>();

            //return await getRepo.GetAll();

            var getRepo = _unitOfWork.GetRepository<Subject>();
            return await getRepo.GetAllIncluding(s => s.Doctor);

        }

        public async Task<Subject> GetSubjectByDoctorId(int id)
        {
            var getRepo = _unitOfWork.GetRepository<Subject>();
            return await getRepo.FindWhen(s => s.DoctorId == id);
        }

        public async Task<IEnumerable<Subject>> GetSubjectByDoctorName(string Name)
        {
            var getRepo = _unitOfWork.GetRepository<Subject>();
            await getRepo.GetAllIncluding(s => s.Doctor); // this line doesn't do anything here
            return await getRepo.FindAllWhen(s => s.Doctor.FullName == Name);
            

        }

        public async Task<Subject> GetSubjectById(int id)
        {
            var getRepo = _unitOfWork.GetRepository<Subject>();
            return await getRepo.GetByID(id);
        }

        public async Task<Subject> GetSubjectByName(string name)
        {
            var getRepo = _unitOfWork.GetRepository<Subject>();
            return await getRepo.FindWhen(s => s.Name == name);
        }
    }
}
