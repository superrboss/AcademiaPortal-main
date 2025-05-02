using AcademiaPortal.Core.DTO;
using AcademiaPortal.Core.Interfaces;
using AcademiaPortal.Core.Models;
using AcademiaPortal.Core.Services;
using AcademiaPortal.Filter;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.IdentityModel.Tokens.Jwt;

namespace AcademiaPortal.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class StudentOriginalController : ControllerBase
    {
        private readonly IStudentServices _studentService;
        private readonly IMapper _automap;

        public StudentOriginalController(IStudentServices studentService, IMapper automap)
        {
            _studentService = studentService;
            _automap = automap;
        }

        [HttpPost]
        public async Task<IActionResult> Registration(StudentDTO newstudent)
        {

            if (newstudent == null)
                return BadRequest("Student data is null");
            // Hash password before mapping
            newstudent.Password = HelpPassword.Hash(newstudent.Password);

            var student = _automap.Map<Student>(newstudent);
            bool result = await _studentService.RegisterStudent(student);

            return result ? Ok("Student registered successfully") : BadRequest("Failed to register student");
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(string StuedentEmail, string Password)
        {
            var CheckUser = await _studentService.LoginStudent(StuedentEmail, Password);

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
        [EnableRateLimiting("fixed")]
        [CheckAuthorizeRole("Admin")]
        [HttpGet("AllStudents")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentService.GetAllStudents();
            if (students == null || !students.Any())
                return NotFound("No students found");

            return Ok(students);
        }
        [HttpGet("AllMyDoctors")]
        public async Task<IActionResult> AllMyDoctors()
        {
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var token = authorizationHeader.Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var UserId = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            var doctors = await _studentService.GetAllMyDoctor(int.Parse(UserId));
            if (doctors == null || !doctors.Any())
                return NotFound("No students found");

            return Ok(doctors);
        }
        [HttpGet("GetStudentById/{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _studentService.GetStudentById(id);
            if (student == null)
                return NotFound("Student not found");

            return Ok(student);
        }
        [HttpPut("EditStudent/{id}")]
        public async Task<IActionResult> EditStudent(int id, [FromBody] StudentDTO studentEdit)
        {
            if (studentEdit == null)
                return BadRequest("Student data is null");

            var existingStudent = await _studentService.GetStudentById(id);
            if (existingStudent == null)
                return NotFound("Student not found by this id");

            _automap.Map(studentEdit, existingStudent);

            if (!string.IsNullOrWhiteSpace(studentEdit.Password))
            {
                existingStudent.Password = HelpPassword.Hash(studentEdit.Password);
            }

            bool result = await _studentService.UpdateStudent(existingStudent);

            return result ? Ok("Student updated successfully") : BadRequest("Failed to update student");
        }
        [HttpDelete("DeleteStudent/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _studentService.GetStudentById(id);
            if (student == null)
                return NotFound("Student not found");

            bool result = await _studentService.DeleteStudent(id);

            return result ? Ok("Student deleted successfully") : BadRequest("Failed to delete student");
        }
        [HttpGet("GetStudentByEmail/{email}")]
        public async Task<IActionResult> GetStudentByEmail(string email)
        {
            var student = await _studentService.GetStudentByEmail(email);
            if (student == null)
                return NotFound("Student not found");

            return Ok(student);
        }
        [HttpGet("GetStudentByName/{name}")]
        public async Task<IActionResult> GetStudentByName(string name)
        {
            var student = await _studentService.GetStudentByName(name);
            if (student == null)
                return NotFound("Student not found");

            return Ok(student);
        }

        [HttpPost("RegisterSubjects")]
        public async Task<IActionResult> RegisterSubjects(int subject_Id)
        {
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var token = authorizationHeader.Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var UserId = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            StudentSubjectDTO AddedSubject = new StudentSubjectDTO()
            {
                StudentId = int.Parse(UserId),
                SubjectId = subject_Id
            };
            var stud = _automap.Map<StudentSubject>(AddedSubject);
            bool result = await _studentService.RegisterSubject(stud);

            return result ? Ok("student_subject registered successfully") : BadRequest("Failed to register this Subject");
        }
       

    }
}