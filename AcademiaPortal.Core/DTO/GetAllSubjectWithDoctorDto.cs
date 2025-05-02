using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaPortal.Core.DTO
{
    public class GetAllSubjectWithDoctorDto
    {
       
            public int Id { get; set; }
            public string Name { get; set; }
            public int DoctorId { get; set; }
            public string DoctorName { get; set; }  // Or other relevant properties
        
    }
}
