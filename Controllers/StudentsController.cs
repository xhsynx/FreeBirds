using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FreeBirds.Data;
using FreeBirds.Models;
using FreeBirds.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace FreeBirds.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AdminOnly")]
    public class StudentsController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
        {
            return await _context.Students
                .Include(s => s.Parent)
                .Select(s => new StudentDTO
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Class = s.Class,
                    Address = s.Address,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    ParentId = s.ParentId,
                    Parent = new ParentDTO
                    {
                        Id = s.Parent.Id,
                        FirstName = s.Parent.FirstName,
                        LastName = s.Parent.LastName,
                        PhoneNumber = s.Parent.PhoneNumber,
                        Email = s.Parent.Email
                    }
                })
                .ToListAsync();
        }


        [HttpGet("{id:guid}")]
        public async Task<ActionResult<StudentDTO>> GetStudent(Guid id)
        {
            var student = await _context.Students
                .Include(s => s.Parent)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            return new StudentDTO
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Class = student.Class,
                Address = student.Address,
                Latitude = student.Latitude,
                Longitude = student.Longitude,
                ParentId = student.ParentId,
                Parent = new ParentDTO
                {
                    Id = student.Parent.Id,
                    FirstName = student.Parent.FirstName,
                    LastName = student.Parent.LastName,
                    PhoneNumber = student.Parent.PhoneNumber,
                    Email = student.Parent.Email
                }
            };
        }


        [HttpPost("create")]
        public async Task<ActionResult<StudentDTO>> CreateStudent(CreateStudentDTO studentDTO)
        {
            var student = new Student
            {
                Id = Guid.NewGuid(),
                FirstName = studentDTO.FirstName,
                LastName = studentDTO.LastName,
                Class = studentDTO.Class,
                Address = studentDTO.Address,
                Latitude = studentDTO.Latitude,
                Longitude = studentDTO.Longitude,
                ParentId = studentDTO.ParentId
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, studentDTO);
        }


        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateStudent(Guid id, CreateStudentDTO studentDTO)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            student.FirstName = studentDTO.FirstName;
            student.LastName = studentDTO.LastName;
            student.Class = studentDTO.Class;
            student.Address = studentDTO.Address;
            student.Latitude = studentDTO.Latitude;
            student.Longitude = studentDTO.Longitude;
            student.ParentId = studentDTO.ParentId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.Students.Any(e => e.Id == id) is false)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}