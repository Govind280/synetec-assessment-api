using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SynetecAssessmentApi.Controllers;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Logging;
using SynetecAssessmentApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Test
{
    [TestClass]
    public class EmployeeControllerTest
    {
        private Mock<IEmployeeService> _mockEmployeeService;
        private Mock<ILogger> _mockLogger;
        private EmployeeController _employeeController;

        [TestInitialize]
        public void Init()
        {
            _mockEmployeeService = new Mock<IEmployeeService>();
            _mockLogger = new Mock<ILogger>();
            _employeeController = new EmployeeController(_mockEmployeeService.Object, _mockLogger.Object);
        }

        [TestMethod]
        public async Task GetAll_Without_Employees()
        {
            // Arrange
            List<EmployeeDto> employeeDtos = null;
            _mockEmployeeService.Setup(a => a.GetEmployeesAsync()).ReturnsAsync(employeeDtos);

            // Act
            var result = await _employeeController.GetAll();

            //Assert
            NotFoundObjectResult actionResult = result as NotFoundObjectResult;
            Assert.AreEqual(404, actionResult.StatusCode);
            Assert.AreEqual("No employees. Please contact Administrator!!", actionResult.Value);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task GetAll_With_Employees()
        {
            // Arrange
            List<EmployeeDto> employeeDtos = new List<EmployeeDto>()
            {
                new() { Fullname = "Employee1", JobTitle="Title 1", Salary = 1000, Department = new() { Description = "Department Description 1", Title = "Department Title 1" } },
                new() { Fullname = "Employee3", JobTitle="Title 3", Salary = 3000, Department = new() { Description = "Department Description 3", Title = "Department Title 3" } },
                new() { Fullname = "Employee2", JobTitle="Title 2", Salary = 2000, Department = new() { Description = "Department Description 2", Title = "Department Title 2" } },
            };

            _mockEmployeeService.Setup(a => a.GetEmployeesAsync()).ReturnsAsync(employeeDtos);

            // Act
            var result = await _employeeController.GetAll();

            //Assert
            OkObjectResult actionResult = result as OkObjectResult;
            List<EmployeeDto> actionResultValue = (List<EmployeeDto>)actionResult.Value;
            Assert.AreEqual(200, actionResult.StatusCode);
            Assert.AreEqual(employeeDtos.Count, actionResultValue.Count);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}
