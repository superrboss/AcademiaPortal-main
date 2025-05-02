using AcademiaPortal.Core.Models;
using AcademiaPortal.Core.Options;
using AcademiaPortal.Core.Roles;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaPortal.Data.UserServies
{
    public class TokenServices
    {
        private readonly JwtMap _jwt;

        public TokenServices(IOptions<JwtMap> jwt)
        {
            _jwt = jwt.Value;
        }
        public JwtSecurityToken GenerateToken(object user)
        {
            string email = "";
            string id = "";
            string role = "";

            if (user is Student student)
            {
                email = student.Email;
                id = student.Id.ToString();
                role = UserRole.Student.ToString();
            }
            else if (user is Doctor doctor)
            {
                email = doctor.Email;
                id = doctor.Id.ToString();
                role = UserRole.Doctor.ToString();
            }
            else if (user is Admin admin)
            {
                email = admin.Email;
                id = admin.Id.ToString();
                role = UserRole.Admin.ToString();
            }
            else
            {
                throw new ArgumentException("Unsupported user type");
            }

            var claims = new List<Claim>
            {
                new Claim("Email", email),
                new Claim("UserId", id),
                new Claim("Role", role),
            };


            // for signature
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                expires: DateTime.Now.AddDays(7),
                claims: claims,
                signingCredentials: signingCredentials
                );
            return token;
        }
    }
}