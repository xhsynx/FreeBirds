using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FreeBirds.DTOs;

namespace FreeBirds.Services
{
    public interface IExamService
    {
        Task<IEnumerable<ExamDTO>> GetAllExamsAsync();
        Task<ExamDTO?> GetExamByIdAsync(Guid id);
        Task<IEnumerable<ExamDTO>> GetExamsByTeacherIdAsync(Guid teacherId);
        Task<IEnumerable<ExamDTO>> GetExamsByClassIdAsync(Guid classId);
        Task<ExamDTO?> CreateExamAsync(CreateExamDTO examDto);
        Task<ExamDTO?> UpdateExamAsync(Guid id, CreateExamDTO examDto);
        Task<bool> DeleteExamAsync(Guid id);
        Task<bool> ToggleExamStatusAsync(Guid id);
    }
} 