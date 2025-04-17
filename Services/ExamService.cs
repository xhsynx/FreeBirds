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
    public class ExamService : IExamService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ExamService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ExamDTO>> GetAllExamsAsync()
        {
            var exams = await _context.Exams
                .Include(e => e.Teacher)
                .Include(e => e.Class)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ExamDTO>>(exams);
        }

        public async Task<ExamDTO?> GetExamByIdAsync(Guid id)
        {
            var exam = await _context.Exams
                .Include(e => e.Teacher)
                .Include(e => e.Class)
                .FirstOrDefaultAsync(e => e.Id == id);

            return exam == null ? null : _mapper.Map<ExamDTO>(exam);
        }

        public async Task<IEnumerable<ExamDTO>> GetExamsByTeacherIdAsync(Guid teacherId)
        {
            var exams = await _context.Exams
                .Include(e => e.Teacher)
                .Include(e => e.Class)
                .Where(e => e.TeacherId == teacherId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ExamDTO>>(exams);
        }

        public async Task<IEnumerable<ExamDTO>> GetExamsByClassIdAsync(Guid classId)
        {
            var exams = await _context.Exams
                .Include(e => e.Teacher)
                .Include(e => e.Class)
                .Where(e => e.ClassId == classId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ExamDTO>>(exams);
        }

        public async Task<ExamDTO?> CreateExamAsync(CreateExamDTO examDto)
        {
            var exam = _mapper.Map<Exam>(examDto);
            exam.Id = Guid.NewGuid();
            exam.CreatedAt = DateTime.UtcNow;
            exam.IsActive = true;

            _context.Exams.Add(exam);
            await _context.SaveChangesAsync();

            return _mapper.Map<ExamDTO>(exam);
        }

        public async Task<ExamDTO?> UpdateExamAsync(Guid id, CreateExamDTO examDto)
        {
            var exam = await _context.Exams.FindAsync(id);
            if (exam == null)
                return null;

            _mapper.Map(examDto, exam);
            exam.LastModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return _mapper.Map<ExamDTO>(exam);
        }

        public async Task<bool> DeleteExamAsync(Guid id)
        {
            var exam = await _context.Exams.FindAsync(id);
            if (exam == null)
                return false;

            _context.Exams.Remove(exam);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ToggleExamStatusAsync(Guid id)
        {
            var exam = await _context.Exams.FindAsync(id);
            if (exam == null)
                return false;

            exam.IsActive = !exam.IsActive;
            exam.LastModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
} 