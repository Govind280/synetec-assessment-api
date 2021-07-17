using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SynetecAssessmentApi.Controllers;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Logging;
using SynetecAssessmentApi.Services;

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
        public async void CalculateBonus_Invalid_EmployeeId_Less_Than_One()
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
    }
}
