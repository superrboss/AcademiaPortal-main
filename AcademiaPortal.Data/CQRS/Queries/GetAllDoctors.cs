using AcademiaPortal.Core.DTO;
using AcademiaPortal.Core.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaPortal.Data.CQRS.Queries
{
    public record GetAllDoctors:IRequest<IEnumerable<GetAllDoctorsDTO>>
    {
    }
}
