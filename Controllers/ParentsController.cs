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
    public class ParentsController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<ParentDTO>>> GetParents()
        {
            return await _context.Parents
                .Select(p => new ParentDTO
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    PhoneNumber = p.PhoneNumber,
                    Email = p.Email
                })
                .ToListAsync();
        }


        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ParentDTO>> GetParent(Guid id)
        {
            var parent = await _context.Parents.FindAsync(id);

            if (parent == null)
            {
                return NotFound();
            }

            return new ParentDTO
            {
                Id = parent.Id,
                FirstName = parent.FirstName,
                LastName = parent.LastName,
                PhoneNumber = parent.PhoneNumber,
                Email = parent.Email
            };
        }


        [HttpPost("create")]
        public async Task<ActionResult<ParentDTO>> CreateParent(CreateParentDTO parentDTO)
        {
            var parent = new Parent
            {
                Id = Guid.NewGuid(),
                FirstName = parentDTO.FirstName,
                LastName = parentDTO.LastName,
                PhoneNumber = parentDTO.PhoneNumber,
                Email = parentDTO.Email
            };

            _context.Parents.Add(parent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetParent), new { id = parent.Id }, parentDTO);
        }


        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateParent(Guid id, CreateParentDTO parentDTO)
        {
            var parent = await _context.Parents.FindAsync(id);
            if (parent == null)
            {
                return NotFound();
            }

            parent.FirstName = parentDTO.FirstName;
            parent.LastName = parentDTO.LastName;
            parent.PhoneNumber = parentDTO.PhoneNumber;
            parent.Email = parentDTO.Email;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.Parents.Any(e => e.Id == id) is false)
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
        public async Task<IActionResult> DeleteParent(Guid id)
        {
            var parent = await _context.Parents.FindAsync(id);
            if (parent is null)
            {
                return NotFound();
            }

            _context.Parents.Remove(parent);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}