using AcademiaPortal.Core.DTO;
using AcademiaPortal.Core.Models;
using AutoMapper;

namespace AcademiaPortal.Core.Mapper
{
    public class AutoMap: Profile
    {
        public AutoMap() {
            CreateMap<Doctor, DoctorDTO>(); // Mapping from Model to DTO
            CreateMap<DoctorDTO, Doctor>(); // Mapping from DTO to Model

            CreateMap<Doctor, GetAllDoctorsDTO>(); // Mapping from Model to DTO
            CreateMap<GetAllDoctorsDTO, Doctor>(); // Mapping from DTO to Model
            
            CreateMap<Doctor, DoctorEdit>(); // Mapping from Model to DTO
            CreateMap<DoctorEdit, Doctor>(); // Mapping from DTO to Model
            CreateMap<Subject, SubjectDTO>(); // Mapping from Model to DTO
            CreateMap<SubjectDTO, Subject>(); // Mapping from DTO to Model
            CreateMap<Subject, GetAllSubjectWithDoctorDto>()
          .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => "DR "+src.Doctor.FullName));
            CreateMap<Student, StudentDTO>(); // Mapping from Model to DTO
            CreateMap<StudentDTO, Student>(); // Mapping from DTO to Model
            CreateMap<StudentSubject, StudentSubjectDTO>(); // Mapping from Model to DTO
            CreateMap<StudentSubjectDTO, StudentSubject>(); // Mapping from DTO to Model
        }
    }
}
