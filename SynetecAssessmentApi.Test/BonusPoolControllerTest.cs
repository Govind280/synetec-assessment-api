using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SynetecAssessmentApi.Controllers;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Logging;
using SynetecAssessmentApi.Services;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Test
{
    [TestClass]
    public class BonusPoolControllerTest
    {
        private Mock<IBonusPoolService> _mockBonusPoolService;
        private Mock<ILogger> _mockLogger;
        private BonusPoolController _bonusPoolController;

        [TestInitialize]
        public void Init()
        {
            _mockBonusPoolService = new Mock<IBonusPoolService>();
            _mockLogger = new Mock<ILogger>();
            _bonusPoolController = new BonusPoolController(_mockBonusPoolService.Object, _mockLogger.Object);
        }

        [TestMethod]
        public async Task CalculateBonus_Invalid_EmployeeId_Less_Than_One()
        {
            // Arrange
            _mockBonusPoolService.Setup(a => a.CalculateAsync(10000, 0)).ReturnsAsync(new BonusPoolCalculatorResultDto
            {
                Amount = 0
            });

            // Act
            var result = await _bonusPoolController.CalculateBonus(new CalculateBonusDto()
            {
                SelectedEmployeeId = 0,
                TotalBonusPoolAmount = 10000
            });

            // Assert
            BadRequestObjectResult actionResult = result as BadRequestObjectResult;
            Assert.AreEqual(400, actionResult.StatusCode);
            Assert.AreEqual("SelectedEmployeeId is not a valid Positive Integer. Please try again with valid SelectedEmployeeId.", actionResult.Value);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task CalculateBonus_Invalid_EmployeeId_DoesNot_Exist()
        {
            int employeeID = 100;
            BonusPoolCalculatorResultDto bonusPoolCalculatorResultDto = null;

            // Arrange
            _mockBonusPoolService.Setup(a => a.CalculateAsync(10000, employeeID)).ReturnsAsync(bonusPoolCalculatorResultDto);

            // Act
            var result = await _bonusPoolController.CalculateBonus(new CalculateBonusDto()
            {
                SelectedEmployeeId = employeeID,
                TotalBonusPoolAmount = 10000
            });

            // Assert
            BadRequestObjectResult actionResult = result as BadRequestObjectResult;
            Assert.AreEqual(400, actionResult.StatusCode);
            Assert.AreEqual($"Invalid SelectedEmployeeId : {employeeID}, Employee doesnt exist. " +
                        $"Please try again with valid SelectedEmployeeId.", actionResult.Value);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task CalculateBonus_Valid_EmployeeId_Amount()
        {
            // Arrange
            int employeeID = 1;
            var expectedResult = new BonusPoolCalculatorResultDto
            {
                Amount = 10,
                Employee = new()
                {
                    Department = new() { Description = "Test Department", Title = "Test Title" },
                    Fullname = "Mr Test",
                    JobTitle = "Test Job",
                    Salary = 1000
                }
            };
            
            _mockBonusPoolService.Setup(a => a.CalculateAsync(10000, employeeID)).ReturnsAsync(expectedResult);

            // Act
            var result = await _bonusPoolController.CalculateBonus(new CalculateBonusDto()
            {
                SelectedEmployeeId = employeeID,
                TotalBonusPoolAmount = 10000
            });

            // Assert
            OkObjectResult actionResult = result as OkObjectResult;
            BonusPoolCalculatorResultDto actionResultValue = (BonusPoolCalculatorResultDto)actionResult.Value;
            Assert.AreEqual(200, actionResult.StatusCode);
            Assert.AreEqual(expectedResult.Amount, actionResultValue.Amount);
            Assert.AreEqual(expectedResult.Employee.Fullname, actionResultValue.Employee.Fullname);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}
