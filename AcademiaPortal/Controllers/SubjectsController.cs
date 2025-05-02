using AcademiaPortal.Core.DTO;
using AcademiaPortal.Core.Interfaces;
using AcademiaPortal.Core.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace AcademiaPortal.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectServices  _services;
        private readonly IMapper _automap;
        public SubjectsController(ISubjectServices services,IMapper mapper)
        {
            _services = services;
            _automap = mapper;
        }

        [HttpGet("GetAllSubjects")]
        public async Task<IActionResult> GetAllSubjects()
        {
            var subjects = await _services.GetAllSubjects();

            if (subjects == null)
                return NotFound("No subjects found");

            var subjectDtos = _automap.Map<List<GetAllSubjectWithDoctorDto>>(subjects);
            return Ok(subjectDtos);

        }

        [HttpGet("GetSubjectById/{id}")]
        public async Task<IActionResult> GetSubjectById(int id)
        {
            var subject = await _services.GetSubjectById(id);
            var subjectDtos = _automap.Map<GetAllSubjectWithDoctorDto>(subject);

            if (subject == null)
                return NotFound("Subject not found");
            return Ok(subjectDtos);
        }
        [HttpPost("AddSubject")]
        public async Task<IActionResult> AddSubject([FromBody] SubjectDTO subject)
        {

            if (subject == null)
                return BadRequest("Subject data is null");
            
            var newSubject = _automap.Map<Subject>(subject);
            bool result = await _services.AddSubject(newSubject);
            return result ? Ok("Subject added successfully") : BadRequest("Failed to add subject");
        }
        [HttpDelete("DeleteSubject/{id}")]
        public async Task<IActionResult> DeleteSubjectById(int id)
        {
            var oldSubject= await _services.GetSubjectById(id);
            if (oldSubject == null)
                return NotFound("Not found Subject have this id !");
            var result = await _services.DeleteSubjectById(id);
            if (!result)
                return NotFound("Subject not found");
            return Ok("Subject deleted successfully");
        }
        [HttpGet("GetSubjectByDoctorid/{id}")]
        public async Task<IActionResult> GetSubjectbyDoctorId(int id)
        {
             var subject = await _services.GetSubjectByDoctorId(id);
            var subjectDtos = _automap.Map<GetAllSubjectWithDoctorDto>(subject);

            if (subject == null)
                return NotFound("Be Sure Doctor Id!");
            return Ok(subjectDtos);
        }
        [HttpGet("GetSubjectByDoctorName/{Name}")]
        public async Task<IActionResult> GetSubjectbyDoctorName(string Name)
        {
            var subject = await _services.GetSubjectByDoctorName(Name);
            //var subjectDtos = _automap.Map<GetAllSubjectWithDoctorDto>(subject);

            if (subject == null)
                return NotFound("Be Sure Doctor Name!");
            var subjectDtos = _automap.Map<IEnumerable<GetAllSubjectWithDoctorDto>>(subject);
            return Ok(subjectDtos);
        }
        [HttpGet("GetAllMySubjects")]
        public async Task<IActionResult> GetAllMySubjects()
        {
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var token = authorizationHeader.Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var UserId = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            var studentSubjects = await _services.GetAllMySubject(int.Parse(UserId));
            if (studentSubjects == null)
                return NotFound("No subjects found for this student");

            return Ok(studentSubjects);
        }

    }
}
