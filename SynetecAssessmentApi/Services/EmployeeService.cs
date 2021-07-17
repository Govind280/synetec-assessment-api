using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SynetecAssessmentApi.Domain;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Logging;
using SynetecAssessmentApi.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Services
{
    /// <summary>
    /// EmployeeService class
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for <see cref="EmployeeService"/>
        /// </summary>
        /// <param name="dbContext">DI for <see cref="AppDbContext"/></param>
        /// <param name="logger">DI for <see cref="ILogger"/></param>
        /// <param name="mapper">DI for <see cref="IMapper"/></param>
        public EmployeeService(AppDbContext dbContext, ILogger logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets employee details
        /// </summary>
        /// <returns>Collection of <see cref="EmployeeDto"/></returns>
        public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync()
        {
            try
            {
                IEnumerable<Employee> employees = await _dbContext
                    .Employees
                    .Include(e => e.Department)
                    .ToListAsync();

                List<EmployeeDto> employeesResult;

                if (employees?.Any() == true)
                {
                    employeesResult = new();
                    employees.ToList().ForEach(emp => employeesResult.Add(_mapper.Map<EmployeeDto>(emp)));

                    return employeesResult;
                }

                return null;
            }
            catch(Exception ex)
            {
                _logger.Error($"Exception while Fetching Employees details. Exception details : {ex}");
                throw;
            }
        }

        /// <summary>
        /// Get's Employee details based in employee ID
        /// </summary>
        /// <param name="selectedEmployeeId">Employee Id</param>
        /// <returns><see cref="Employee"/></returns>
        public async Task<Employee> GetEmployeeById(int selectedEmployeeId) => await _dbContext.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(item => item.Id == selectedEmployeeId);

        /// <summary>
        /// Sum of all employee salary
        /// </summary>
        /// <returns>Sum of Employees salary</returns>
        public async Task<int> GetTotalEmployeeSalary() => await _dbContext.Employees.SumAsync(item => item.Salary);
    }
}
