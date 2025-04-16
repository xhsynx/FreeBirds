using Microsoft.AspNetCore.Mvc;
using FreeBirds.Services;
using FreeBirds.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FreeBirds.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class StudentController : ControllerBase
    {
        private readonly StudentService _studentService;

        public StudentController(StudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Student>>> GetAllStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudentById(Guid id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Student>> CreateStudent(Student student)
        {
            var createdStudent = await _studentService.CreateStudentAsync(student);
            return CreatedAtAction(nameof(GetStudentById), new { id = createdStudent.Id }, createdStudent);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Student>> UpdateStudent(Guid id, Student student)
        {
            var updatedStudent = await _studentService.UpdateStudentAsync(id, student);
            if (updatedStudent == null)
            {
                return NotFound();
            }
            return Ok(updatedStudent);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var result = await _studentService.DeleteStudentAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("type/{type}")]
        public async Task<ActionResult<List<Student>>> GetStudentsByType(StudentType type)
        {
            var students = await _studentService.GetStudentsByTypeAsync(type);
            return Ok(students);
        }

        [HttpGet("active")]
        public async Task<ActionResult<List<Student>>> GetActiveStudents()
        {
            var students = await _studentService.GetActiveStudentsAsync();
            return Ok(students);
        }

        [HttpGet("expiring/{days}")]
        public async Task<ActionResult<List<Student>>> GetStudentsWithExpiringRegistration(int days)
        {
            var students = await _studentService.GetStudentsWithExpiringRegistrationAsync(days);
            return Ok(students);
        }
    }
} 