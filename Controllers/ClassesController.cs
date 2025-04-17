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
    public class ClassesController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<ClassDTO>>> GetClasses()
        {
            return await _context.Classes
                .Include(c => c.ClassTeacher)
                .Select(c => new ClassDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Grade = c.Grade,
                    Section = c.Section,
                    Capacity = c.Capacity,
                    CurrentStudentCount = c.CurrentStudentCount,
                    ClassTeacherId = c.ClassTeacherId,
                    ClassTeacher = c.ClassTeacher != null ? new TeacherDTO
                    {
                        Id = c.ClassTeacher.Id,
                        FirstName = c.ClassTeacher.FirstName,
                        LastName = c.ClassTeacher.LastName,
                        PhoneNumber = c.ClassTeacher.PhoneNumber,
                        Email = c.ClassTeacher.Email,
                        Subject = c.ClassTeacher.Subject,
                        Address = c.ClassTeacher.Address,
                        Latitude = c.ClassTeacher.Latitude,
                        Longitude = c.ClassTeacher.Longitude,
                        IsActive = c.ClassTeacher.IsActive,
                        CreatedAt = c.ClassTeacher.CreatedAt,
                        LastModifiedAt = c.ClassTeacher.LastModifiedAt
                    } : null,
                    IsActive = c.IsActive,
                    CreatedAt = c.CreatedAt,
                    LastModifiedAt = c.LastModifiedAt
                })
                .ToListAsync();
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ClassDTO>> GetClass(Guid id)
        {
            var classEntity = await _context.Classes
                .Include(c => c.ClassTeacher)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (classEntity is null)
            {
                return NotFound();
            }

            return new ClassDTO
            {
                Id = classEntity.Id,
                Name = classEntity.Name,
                Grade = classEntity.Grade,
                Section = classEntity.Section,
                Capacity = classEntity.Capacity,
                CurrentStudentCount = classEntity.CurrentStudentCount,
                ClassTeacherId = classEntity.ClassTeacherId,
                ClassTeacher = classEntity.ClassTeacher != null ? new TeacherDTO
                {
                    Id = classEntity.ClassTeacher.Id,
                    FirstName = classEntity.ClassTeacher.FirstName,
                    LastName = classEntity.ClassTeacher.LastName,
                    PhoneNumber = classEntity.ClassTeacher.PhoneNumber,
                    Email = classEntity.ClassTeacher.Email,
                    Subject = classEntity.ClassTeacher.Subject,
                    Address = classEntity.ClassTeacher.Address,
                    Latitude = classEntity.ClassTeacher.Latitude,
                    Longitude = classEntity.ClassTeacher.Longitude,
                    IsActive = classEntity.ClassTeacher.IsActive,
                    CreatedAt = classEntity.ClassTeacher.CreatedAt,
                    LastModifiedAt = classEntity.ClassTeacher.LastModifiedAt
                } : null,
                IsActive = classEntity.IsActive,
                CreatedAt = classEntity.CreatedAt,
                LastModifiedAt = classEntity.LastModifiedAt
            };
        }

        [HttpPost("create")]
        public async Task<ActionResult<ClassDTO>> CreateClass(CreateClassDTO classDTO)
        {
            var classEntity = new Class
            {
                Id = Guid.NewGuid(),
                Name = classDTO.Name,
                Grade = classDTO.Grade,
                Section = classDTO.Section,
                Capacity = classDTO.Capacity,
                CurrentStudentCount = 0,
                ClassTeacherId = classDTO.ClassTeacherId
            };

            _context.Classes.Add(classEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClass), new { id = classEntity.Id }, classDTO);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateClass(Guid id, CreateClassDTO classDTO)
        {
            var classEntity = await _context.Classes.FindAsync(id);
            if (classEntity is null)
            {
                return NotFound();
            }

            classEntity.Name = classDTO.Name;
            classEntity.Grade = classDTO.Grade;
            classEntity.Section = classDTO.Section;
            classEntity.Capacity = classDTO.Capacity;
            classEntity.ClassTeacherId = classDTO.ClassTeacherId;
            classEntity.LastModifiedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.Classes.Any(e => e.Id == id) is false)
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
        public async Task<IActionResult> DeleteClass(Guid id)
        {
            var classEntity = await _context.Classes.FindAsync(id);
            if (classEntity is null)
            {
                return NotFound();
            }

            _context.Classes.Remove(classEntity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
} 