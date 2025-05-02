using AcademiaPortal.Core.DTO;
using AcademiaPortal.Core.Interfaces;
using AcademiaPortal.Core.Models;
using AcademiaPortal.Core.Services;
using AcademiaPortal.Data.CQRS.Commands;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using MediatR;
using AcademiaPortal.Data.CQRS.Queries;


namespace AcademiaPortal.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorServices _doctorServices;
        private readonly IMapper _AutoMap;
        private readonly IMediator _mediatr;

        public DoctorsController(IDoctorServices doctorServices, IMapper AutoMap, IMediator mediatr)
        {
            _doctorServices = doctorServices;
            _AutoMap = AutoMap;
            _mediatr = mediatr;
        }

        // POST: api/Doctors/AddDoctor
        [HttpPost("AddDoctor")]
        public async Task<IActionResult> AddDoctor([FromBody] DoctorDTO doctorDTO)
        {
            if (doctorDTO == null)
                return BadRequest("Doctor data is null");
            // Hash password before mapping
            doctorDTO.Password = HelpPassword.Hash(doctorDTO.Password);
            
            var newDoc = _AutoMap.Map<Doctor>(doctorDTO);

            bool result = await _mediatr.Send(new InsertDoctorsCommand(newDoc)); 
            //InsertDoctorsCommand

            return result ? Ok("Doctor added successfully") : BadRequest("Failed to add doctor");

        }
        [HttpGet("GetDoctorById/{id}")]

        public async Task<IActionResult> GetDoctorById(int id)
        {
            Doctor doctor = await _doctorServices.GetDoctorById(id);
            if (doctor == null)
                return NotFound("Doctor not found");
            return Ok(doctor);
        }
        [HttpGet("GetAllDoctor")]
        //[CheckAuthorizeRole("Admin")]

        public async Task<IActionResult> GetAllDoctor()
        {
            //IEnumerable<GetAllDoctorsDTO> AllDoctor = await _doctorServices.GetAllDoctors();
            IEnumerable<GetAllDoctorsDTO> AllDoctor = await _mediatr.Send(new GetAllDoctors());
            if (AllDoctor == null)
                return NotFound("No doctors found");
            return Ok(AllDoctor);
        }
        [HttpDelete("DeletedById/{id}")]
        public async Task<IActionResult> DeletedDoctorById(int id)
        {
            bool doctor = await _doctorServices.DeleteDocftorById(id);
            if (!doctor)
                return NotFound("Doctor not found");
            return Ok("Doctor deleted successfully");
        }
        [HttpGet("GetDoctorByName/{name}")]
        public async Task<IActionResult> GetDoctorByName(string name)
        {
            Doctor doctor = await _doctorServices.GetDoctorByName(name);
            if (doctor == null)
                return NotFound("Doctor not found");
            return Ok(doctor);
        }
      
        [HttpPut("EditDoctor/{id}")]
        public async Task<IActionResult> EditDoctor(int id, [FromBody] DoctorDTO DoctorEdit)
        {
            if (DoctorEdit == null)
                return BadRequest("Doctor data is null");

            var existingDoctor = await _doctorServices.GetDoctorById(id);
            if (existingDoctor == null)
                return NotFound("Doctor not found by this id");

            _AutoMap.Map(DoctorEdit, existingDoctor);

            if (!string.IsNullOrWhiteSpace(DoctorEdit.Password))
            {
                existingDoctor.Password = HelpPassword.Hash(DoctorEdit.Password);
            }

            bool result = await _doctorServices.EditDoctor(existingDoctor);

            return result ? Ok("Doctor updated successfully") : BadRequest("Failed to update doctor");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string DoctorEmail, string Password)
        {
            var CheckUser = await _doctorServices.LoginDoctor(DoctorEmail, Password);

            if (CheckUser != null)
                return NotFound(CheckUser);

            Response.Headers["Token"] = CheckUser;
            return Ok(new { Token = CheckUser });
        }

        [HttpGet("GetClaim")]
        public IActionResult GetClaimByHeader()
        {
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var token = authorizationHeader.Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
            var UserId = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            var MyRole = jwtToken.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            return Ok($"email = {email}  UserId= {UserId} MyRole= {MyRole}");
        }
        [HttpGet("GetAllMyStudents")]
        public async Task<IActionResult> GetAllMyStudents()
        {
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var token = authorizationHeader.Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var UserId = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            var students = await _doctorServices.GetAllMyStudents(int.Parse(UserId));

            if (students == null || !students.Any())
            {
                return NotFound("No students found for this doctor.");
            }

            return Ok(students);
        }
    }
}
 