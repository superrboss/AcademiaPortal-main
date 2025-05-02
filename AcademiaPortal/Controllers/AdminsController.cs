using AcademiaPortal.Core.Interfaces;
using AcademiaPortal.Data.Implements;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademiaPortal.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AdminsController : ControllerBase
    {
        private readonly IAdminServices _Services;
        private readonly IMapper _AutoMap;

        public AdminsController(IAdminServices Services, IMapper AutoMap)
        {
            _AutoMap = AutoMap;
            _Services = Services;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var CheckUser = await _Services.LoginAdmin(email, password);

            if (CheckUser != null)
                return NotFound(CheckUser);

            Response.Headers["Token"] = CheckUser;
            return Ok(new { Token = CheckUser });
        }


    }
}
