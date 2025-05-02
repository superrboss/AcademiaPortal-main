using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace AcademiaPortal.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]

    public class CheckAuthorizeRole : Attribute, IAuthorizationFilter
    {
        private readonly string[] _roles;

        public CheckAuthorizeRole(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {

            var header = context.HttpContext.Request.Headers["Authorization"].ToString();
            if (header == null || string.IsNullOrEmpty(header))
            {
                context.Result = new UnauthorizedResult(); // 401 Unauthorized
                return;
            }

            Console.WriteLine($"header== {header}");

            var handler = new JwtSecurityTokenHandler();

            var jwtToken = handler.ReadJwtToken(header);

            List<string> roles = jwtToken.Claims.Where(c => c.Type == "Role").Select(c => c.Value).ToList();

            foreach (var role in roles)
            {
                if (roles.Contains(role))
                    Console.WriteLine(role);
            }

            if (!_roles.Any(role => roles.Contains(role)))
            {
                context.Result = new ObjectResult(new { message = "You are not authorized" });
                context.Result = new ForbidResult(); // 403 Forbidden
                return;
            }

        }
    }
}
