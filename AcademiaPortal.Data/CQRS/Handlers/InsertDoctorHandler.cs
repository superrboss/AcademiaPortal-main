using AcademiaPortal.Core.Interfaces;
using AcademiaPortal.Data.CQRS.Commands;
using MediatR;

namespace AcademiaPortal.Data.CQRS.Handlers
{
    public class InsertDoctorHandler : IRequestHandler<InsertDoctorsCommand, bool>
    {
        private readonly IDoctorServices _servies;
        public InsertDoctorHandler(IDoctorServices servies)
        {
            _servies = servies;
        }

        public async Task<bool> Handle(InsertDoctorsCommand request, CancellationToken cancellationToken)
        {
            return await _servies.AddDoctor(request.DoctorAdded);
        }

       
    }
}
