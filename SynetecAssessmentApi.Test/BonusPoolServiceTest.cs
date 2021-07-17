using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SynetecAssessmentApi.Domain;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Logging;
using SynetecAssessmentApi.Services;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Test
{
    [TestClass]
    public class BonusPoolServiceTest
    {
        private Mock<ILogger> _mockLogger;
        private Mock<IMapper> _mockMapper;
        private Mock<IEmployeeService> _mockEmployeeService;
        private BonusPoolService _bonusPoolService;

        [TestInitialize]
        public void Init()
        {
            _mockLogger = new Mock<ILogger>();
            _mockMapper = new Mock<IMapper>();
            _mockEmployeeService = new Mock<IEmployeeService>();
            _bonusPoolService = new BonusPoolService(_mockLogger.Object, _mockMapper.Object, _mockEmployeeService.Object);
        }

        [TestMethod]
        public async Task CalculateAsync_Valid_Details()
        {
            // Arrange
            _mockEmployeeService.Setup(a => a.GetEmployeeById(1)).ReturnsAsync(GetEmployee());
            _mockEmployeeService.Setup(a=>a.GetTotalEmployeeSalary()).ReturnsAsync(1000);

            // Act
            BonusPoolCalculatorResultDto actionResultValue = await _bonusPoolService.CalculateAsync(1000, 1);

            // Assert
            Assert.AreEqual(100, actionResultValue.Amount);
            Assert.IsNotNull(actionResultValue);
        }

        [TestMethod]
        public async Task CalculateAsync_InValid_Null_Employee()
        {
            // Arrange
            Employee employee = null;

            _mockEmployeeService.Setup(a => a.GetEmployeeById(1)).ReturnsAsync(employee);
            _mockEmployeeService.Setup(a => a.GetTotalEmployeeSalary()).ReturnsAsync(1000);

            // Act
            BonusPoolCalculatorResultDto actionResultValue = await _bonusPoolService.CalculateAsync(1000, 1);

            // Assert
            Assert.IsNull(actionResultValue);
        }

        [TestMethod]
        public async Task CalculateAsync_InValid_Negative_Salary()
        {
            // Arrange
            Employee employee = GetEmployee();
            employee.Salary = -1;

            _mockEmployeeService.Setup(a => a.GetEmployeeById(1)).ReturnsAsync(employee);
            _mockEmployeeService.Setup(a => a.GetTotalEmployeeSalary()).ReturnsAsync(1000);

            // Act
            BonusPoolCalculatorResultDto actionResultValue = await _bonusPoolService.CalculateAsync(1000, 1);

            // Assert
            Assert.AreEqual(0, actionResultValue.Amount);
            Assert.IsNotNull(actionResultValue);
        }

        [TestMethod]
        public async Task CalculateAsync_InValid_Zero_Salary()
        {
            // Arrange
            Employee employee = GetEmployee();
            employee.Salary = 0;

            _mockEmployeeService.Setup(a => a.GetEmployeeById(1)).ReturnsAsync(employee);
            _mockEmployeeService.Setup(a => a.GetTotalEmployeeSalary()).ReturnsAsync(1000);

            // Act
            BonusPoolCalculatorResultDto actionResultValue = await _bonusPoolService.CalculateAsync(1000, 1);

            // Assert
            Assert.AreEqual(0, actionResultValue.Amount);
            Assert.IsNotNull(actionResultValue);
        }

        private Employee GetEmployee() => new Employee(1, "Test", "Developer", 100, 1);
    }
}
