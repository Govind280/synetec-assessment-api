using AutoMapper;
using SynetecAssessmentApi.Domain;
using SynetecAssessmentApi.Dtos;

namespace SynetecAssessmentApi
{
    /// <summary>
    /// MapProfile class
    /// </summary>
    public class MapProfile : Profile
    {
        /// <summary>
        /// Map Profile for automapper
        /// </summary>
        public MapProfile()
        {
            CreateMap<Employee, EmployeeDto>();
            CreateMap<Department, DepartmentDto>();
        }
    }
}
