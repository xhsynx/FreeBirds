using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FreeBirds.Data;
using FreeBirds.Models;

namespace FreeBirds.Services
{
    public class StudentService
    {
        private readonly AppDbContext _context;

        public StudentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(Guid id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<Student> CreateStudentAsync(Student student)
        {
            student.Id = Guid.NewGuid();
            student.CreatedAt = DateTime.UtcNow;
            student.IsActive = true;

            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student?> UpdateStudentAsync(Guid id, Student updatedStudent)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return null;
            }

            student.FirstName = updatedStudent.FirstName;
            student.LastName = updatedStudent.LastName;
            student.StudentType = updatedStudent.StudentType;
            student.Location = updatedStudent.Location;
            student.AvatarUrl = updatedStudent.AvatarUrl;
            student.MotherName = updatedStudent.MotherName;
            student.FatherName = updatedStudent.FatherName;
            student.RegistrationDate = updatedStudent.RegistrationDate;
            student.RegistrationFee = updatedStudent.RegistrationFee;
            student.RegistrationEndDate = updatedStudent.RegistrationEndDate;
            student.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<bool> DeleteStudentAsync(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return false;
            }

            student.IsActive = false;
            student.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Student>> GetStudentsByTypeAsync(StudentType type)
        {
            return await _context.Students
                .Where(s => s.StudentType == type && s.IsActive)
                .ToListAsync();
        }

        public async Task<List<Student>> GetActiveStudentsAsync()
        {
            return await _context.Students
                .Where(s => s.IsActive)
                .ToListAsync();
        }

        public async Task<List<Student>> GetStudentsWithExpiringRegistrationAsync(int daysUntilExpiration)
        {
            var expirationDate = DateTime.UtcNow.AddDays(daysUntilExpiration);
            return await _context.Students
                .Where(s => s.IsActive && s.RegistrationEndDate <= expirationDate)
                .ToListAsync();
        }
    }
} 