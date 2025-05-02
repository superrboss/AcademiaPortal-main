using AcademiaPortal.Core.Models;
using AcademiaPortal.Core.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaPortal.Core.DTO
{
    public class GetAllDoctorsDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; } = UserRole.Doctor;

        public List<string> Subjects { get; set; }
    }
}
