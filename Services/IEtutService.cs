using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FreeBirds.DTOs;

namespace FreeBirds.Services
{
    public interface IEtutService
    {
        Task<IEnumerable<EtutDTO>> GetAllEtutsAsync();
        Task<EtutDTO?> GetEtutByIdAsync(Guid id);
        Task<IEnumerable<EtutDTO>> GetEtutsByTeacherIdAsync(Guid teacherId);
        Task<IEnumerable<EtutDTO>> GetEtutsByClassIdAsync(Guid classId);
        Task<IEnumerable<EtutDTO>> GetEtutsByStudentIdAsync(Guid studentId);
        Task<EtutDTO?> CreateEtutAsync(CreateEtutDTO etutDto);
        Task<EtutDTO?> UpdateEtutAsync(Guid id, CreateEtutDTO etutDto);
        Task<bool> DeleteEtutAsync(Guid id);
        Task<bool> AddStudentToEtutAsync(Guid etutId, Guid studentId);
        Task<bool> RemoveStudentFromEtutAsync(Guid etutId, Guid studentId);
        Task<bool> ToggleEtutStatusAsync(Guid id);
    }
} 