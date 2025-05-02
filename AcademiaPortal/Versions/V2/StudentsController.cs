using AcademiaPortal.Core.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademiaPortal.Versions.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class StudentsController : ControllerBase
    {
        private readonly IStudentServices _studentService;

        public StudentsController(IStudentServices studentService, IMapper automap)
        {
            _studentService = studentService;
        }

        //[ApiVersion("2.0")]
        [HttpGet]
        public async Task<IActionResult> GetAllStudentsv2()
        {
            var students = await _studentService.GetAllStudents();
            if (students == null || !students.Any())
                return NotFound("No students found");

            return Ok("From V2");
        }
    }
}
