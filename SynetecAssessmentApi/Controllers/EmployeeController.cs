using Microsoft.AspNetCore.Mvc;
using SynetecAssessmentApi.Logging;
using SynetecAssessmentApi.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Controllers
{
    /// <summary>
    /// Controller for <see cref="EmployeeController"/>
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor for <see cref="EmployeeController"/>
        /// </summary>
        /// <param name="employeeService"><see cref="IEmployeeService"/></param>
        /// <param name="logger"><see cref="ILogger"/></param>
        public EmployeeController(IEmployeeService employeeService, ILogger logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        /// <summary>
        /// Get All employees details
        /// </summary>
        /// <returns>Collection of Employee details</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var employeesList = await _employeeService.GetEmployeesAsync();

                if (employeesList?.Any() == true)
                    return Ok(employeesList);

                return NotFound("No employees. Please contact Administrator!!");
            }
            catch(Exception ex)
            {
                _logger.Error($"Error while Fetching employee. Exception details : {ex}");
                throw;
            }
        }
    }
}
