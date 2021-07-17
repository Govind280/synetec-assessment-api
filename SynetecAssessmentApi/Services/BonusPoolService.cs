using AutoMapper;
using SynetecAssessmentApi.Domain;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Logging;
using SynetecAssessmentApi.Persistence;
using System;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Services
{
    /// <summary>
    /// BonusPoolService class
    /// </summary>
    public class BonusPoolService : IBonusPoolService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;

        /// <summary>
        /// Constructor for <see cref="BonusPoolService"/>
        /// </summary>
        /// <param name="dbContext">DI for <see cref="AppDbContext"/></param>
        /// <param name="logger">DI for <see cref="ILogger"/></param>
        /// <param name="mapper">DI for <see cref="IMapper"/></param>
        /// <param name="employeeService">DI for <see cref="IEmployeeService"/></param>
        public BonusPoolService(AppDbContext dbContext, ILogger logger, IMapper mapper, IEmployeeService employeeService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
            _employeeService = employeeService;
        }

        /// <summary>
        /// Caculates bonus amount for an employee based on input details
        /// </summary>
        /// <param name="bonusPoolAmount">Total Bonus Amount</param>
        /// <param name="selectedEmployeeId">Employee Id</param>
        /// <returns><see cref="BonusPoolCalculatorResultDto"/></returns>
        public async Task<BonusPoolCalculatorResultDto> CalculateAsync(int bonusPoolAmount, int selectedEmployeeId)
        {
            try
            {
                //Load the details of the selected employee using the Id
                Employee employee = await _employeeService.GetEmployeeById(selectedEmployeeId);

                if(employee == null)
                {
                    _logger.Error("Invalid EmployeeID, selectedEmployeeId doesnt exist");
                    return null;
                }

                if (employee.Salary < 1)
                {
                    _logger.Error($"Invalid Employee configuration. Salary for Employee : {employee.Fullname} " +
                        $"with ID : {employee.Id} is {employee.Salary}");

                    return new BonusPoolCalculatorResultDto() { Amount = 0 };
                }

                //Get the total salary budget for the company
                int totalSalary = await _employeeService.GetTotalEmployeeSalary();

                //Calculate the bonus allocation for the employee
                decimal bonusPercentage = (decimal)employee.Salary / totalSalary;
                int bonusAllocation = (int)(bonusPercentage * bonusPoolAmount);

                return new BonusPoolCalculatorResultDto
                {
                    Amount = bonusAllocation,
                    Employee = _mapper.Map<EmployeeDto>(employee)
                };
            }
            catch(Exception ex)
            {
                _logger.Error($"Error while calculating Bonus for selectedEmployeeId : {selectedEmployeeId}");
                _logger.Error($"ExceptionDetails : {ex}");
                throw;
            }
        }
    }
}
