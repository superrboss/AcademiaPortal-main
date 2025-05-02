using AcademiaPortal.Core.Interfaces;
using AcademiaPortal.Core.Models;
using AcademiaPortal.Core.Options;
using AcademiaPortal.Core.Services;
using AcademiaPortal.Data.UserServies;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaPortal.Data.Implements
{
    public class AdminServices : IAdminServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOptions<JwtMap> _jwt;

        public AdminServices(IUnitOfWork unitOfWork, IOptions<JwtMap> jwt)
        {
            _unitOfWork = unitOfWork;
            _jwt = jwt;
        }

        

        public async Task<Admin> GetAdminByEmail(string email)
        {
            var getRepo = _unitOfWork.GetRepository<Admin>();
            return await getRepo.FindWhen(x => x.Email == email);
        }

        public async Task<string> LoginAdmin(string email, string password)
        {
            Admin SerachAboutAdmin = await GetAdminByEmail(email);
            if (SerachAboutAdmin == null)
                return "NotFound this SerachAboutDoctor";
          
            if (!HelpPassword.Verify(password, SerachAboutAdmin.Password))
                return "Error Password herererererer";

            TokenServices tokenService = new TokenServices(_jwt);
            var token = tokenService.GenerateToken(SerachAboutAdmin);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
