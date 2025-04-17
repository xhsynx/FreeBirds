using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using FreeBirds.Data;
using FreeBirds.Models;
using FreeBirds.DTOs;

namespace FreeBirds.Services
{
    public class EtutService : IEtutService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public EtutService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EtutDTO>> GetAllEtutsAsync()
        {
            var etuts = await _context.Etuts
                .Include(e => e.Teacher)
                .Include(e => e.Class)
                .Include(e => e.Students)
                .ToListAsync();

            return _mapper.Map<IEnumerable<EtutDTO>>(etuts);
        }

        public async Task<EtutDTO?> GetEtutByIdAsync(Guid id)
        {
            var etut = await _context.Etuts
                .Include(e => e.Teacher)
                .Include(e => e.Class)
                .Include(e => e.Students)
                .FirstOrDefaultAsync(e => e.Id == id);

            return etut == null ? null : _mapper.Map<EtutDTO>(etut);
        }

        public async Task<IEnumerable<EtutDTO>> GetEtutsByTeacherIdAsync(Guid teacherId)
        {
            var etuts = await _context.Etuts
                .Include(e => e.Teacher)
                .Include(e => e.Class)
                .Include(e => e.Students)
                .Where(e => e.TeacherId == teacherId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<EtutDTO>>(etuts);
        }

        public async Task<IEnumerable<EtutDTO>> GetEtutsByClassIdAsync(Guid classId)
        {
            var etuts = await _context.Etuts
                .Include(e => e.Teacher)
                .Include(e => e.Class)
                .Include(e => e.Students)
                .Where(e => e.ClassId == classId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<EtutDTO>>(etuts);
        }

        public async Task<IEnumerable<EtutDTO>> GetEtutsByStudentIdAsync(Guid studentId)
        {
            var etuts = await _context.Etuts
                .Include(e => e.Teacher)
                .Include(e => e.Class)
                .Include(e => e.Students)
                .Where(e => e.Students.Any(s => s.Id == studentId))
                .ToListAsync();

            return _mapper.Map<IEnumerable<EtutDTO>>(etuts);
        }

        public async Task<EtutDTO?> CreateEtutAsync(CreateEtutDTO etutDto)
        {
            var etut = _mapper.Map<Etut>(etutDto);
            etut.Id = Guid.NewGuid();
            etut.CreatedAt = DateTime.UtcNow;
            etut.IsActive = true;

            _context.Etuts.Add(etut);
            await _context.SaveChangesAsync();

            return _mapper.Map<EtutDTO>(etut);
        }

        public async Task<EtutDTO?> UpdateEtutAsync(Guid id, CreateEtutDTO etutDto)
        {
            var etut = await _context.Etuts.FindAsync(id);
            if (etut == null)
                return null;

            _mapper.Map(etutDto, etut);
            etut.LastModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return _mapper.Map<EtutDTO>(etut);
        }

        public async Task<bool> DeleteEtutAsync(Guid id)
        {
            var etut = await _context.Etuts.FindAsync(id);
            if (etut == null)
                return false;

            _context.Etuts.Remove(etut);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddStudentToEtutAsync(Guid etutId, Guid studentId)
        {
            var etut = await _context.Etuts
                .Include(e => e.Students)
                .FirstOrDefaultAsync(e => e.Id == etutId);

            var student = await _context.Students.FindAsync(studentId);

            if (etut == null || student == null)
                return false;

            if (etut.Students.Any(s => s.Id == studentId))
                return false;

            etut.Students.Add(student);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveStudentFromEtutAsync(Guid etutId, Guid studentId)
        {
            var etut = await _context.Etuts
                .Include(e => e.Students)
                .FirstOrDefaultAsync(e => e.Id == etutId);

            if (etut == null)
                return false;

            var student = etut.Students.FirstOrDefault(s => s.Id == studentId);
            if (student == null)
                return false;

            etut.Students.Remove(student);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ToggleEtutStatusAsync(Guid id)
        {
            var etut = await _context.Etuts.FindAsync(id);
            if (etut == null)
                return false;

            etut.IsActive = !etut.IsActive;
            etut.LastModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
} 