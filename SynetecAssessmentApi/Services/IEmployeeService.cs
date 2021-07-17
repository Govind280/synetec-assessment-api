using SynetecAssessmentApi.Domain;
using SynetecAssessmentApi.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Services
{
    /// <summary>
    /// Interface for EmployeeService
    /// </summary>
    public interface IEmployeeService
    {
        /// <summary>
        /// Gets employee details
        /// </summary>
        /// <returns>Collection of <see cref="EmployeeDto"/></returns>
        Task<IEnumerable<EmployeeDto>> GetEmployeesAsync();

        Task<Employee> GetEmployeeById(int selectedEmployeeId);

        Task<int> GetTotalEmployeeSalary();
    }
}
