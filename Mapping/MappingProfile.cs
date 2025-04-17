using AutoMapper;
using FreeBirds.Models;
using FreeBirds.DTOs;

namespace FreeBirds.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Etut mappings
            CreateMap<Etut, EtutDTO>();
            CreateMap<CreateEtutDTO, Etut>();

            // Teacher mappings
            CreateMap<Teacher, TeacherDTO>();

            // Class mappings
            CreateMap<Class, ClassDTO>();

            // Student mappings
            CreateMap<Student, StudentDTO>();
        }
    }
} 