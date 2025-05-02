using AcademiaPortal.Core.DTO;
using AcademiaPortal.Core.Interfaces;
using AcademiaPortal.Core.Models;
using AcademiaPortal.Data.CQRS.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaPortal.Data.CQRS.Handlers
{
    public class GetAllDoctorsHandler : IRequestHandler<GetAllDoctors, IEnumerable<GetAllDoctorsDTO>>
    {
        private readonly IDoctorServices _servies;
        public GetAllDoctorsHandler(IDoctorServices servies)
        {
            _servies = servies;
        }

        public async Task<IEnumerable<GetAllDoctorsDTO>> Handle(GetAllDoctors request, CancellationToken cancellationToken)
        {
            return await _servies.GetAllDoctors();
        }

       
    }
    
}
