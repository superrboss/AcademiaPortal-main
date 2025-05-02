using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaPortal.Core.Services
{
    public static class HelpPassword
    {
        public static string Hash(string password)
        {
            return Convert.ToBase64String(
                System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password))
                );
        }

        public static bool Verify(string password, string hashedPassword)
        {
            var hashedInputPassword = Hash(password);
            return hashedInputPassword == hashedPassword;
        }
    }
}
