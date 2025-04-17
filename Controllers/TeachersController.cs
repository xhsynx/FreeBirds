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
    public class TeachersController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<TeacherDTO>>> GetTeachers()
        {
            return await _context.Teachers
                .Select(t => new TeacherDTO
                {
                    Id = t.Id,
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    PhoneNumber = t.PhoneNumber,
                    Email = t.Email,
                    Subject = t.Subject,
                    Address = t.Address,
                    Latitude = t.Latitude,
                    Longitude = t.Longitude,
                    IsActive = t.IsActive,
                    CreatedAt = t.CreatedAt,
                    LastModifiedAt = t.LastModifiedAt
                })
                .ToListAsync();
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TeacherDTO>> GetTeacher(Guid id)
        {
            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher is null)
            {
                return NotFound();
            }

            return new TeacherDTO
            {
                Id = teacher.Id,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                PhoneNumber = teacher.PhoneNumber,
                Email = teacher.Email,
                Subject = teacher.Subject,
                Address = teacher.Address,
                Latitude = teacher.Latitude,
                Longitude = teacher.Longitude,
                IsActive = teacher.IsActive,
                CreatedAt = teacher.CreatedAt,
                LastModifiedAt = teacher.LastModifiedAt
            };
        }

        [HttpPost("create")]
        public async Task<ActionResult<TeacherDTO>> CreateTeacher(CreateTeacherDTO teacherDTO)
        {
            var teacher = new Teacher
            {
                Id = Guid.NewGuid(),
                FirstName = teacherDTO.FirstName,
                LastName = teacherDTO.LastName,
                PhoneNumber = teacherDTO.PhoneNumber,
                Email = teacherDTO.Email,
                Subject = teacherDTO.Subject,
                Address = teacherDTO.Address,
                Latitude = teacherDTO.Latitude,
                Longitude = teacherDTO.Longitude
            };

            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTeacher), new { id = teacher.Id }, teacherDTO);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTeacher(Guid id, CreateTeacherDTO teacherDTO)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher is null)
            {
                return NotFound();
            }

            teacher.FirstName = teacherDTO.FirstName;
            teacher.LastName = teacherDTO.LastName;
            teacher.PhoneNumber = teacherDTO.PhoneNumber;
            teacher.Email = teacherDTO.Email;
            teacher.Subject = teacherDTO.Subject;
            teacher.Address = teacherDTO.Address;
            teacher.Latitude = teacherDTO.Latitude;
            teacher.Longitude = teacherDTO.Longitude;
            teacher.LastModifiedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.Teachers.Any(e => e.Id == id) is false)
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
        public async Task<IActionResult> DeleteTeacher(Guid id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher is null)
            {
                return NotFound();
            }

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
} 