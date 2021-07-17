using Microsoft.AspNetCore.Mvc;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Logging;
using SynetecAssessmentApi.Services;
using System;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Controllers
{
    /// <summary>
    /// Controller for <see cref="BonusPoolController"/>
    /// </summary>
    [Route("api/[controller]")]
    public class BonusPoolController : Controller
    {
        private readonly IBonusPoolService _bonusPoolService;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor for <see cref="BonusPoolController"/>
        /// </summary>
        /// <param name="bonusPoolService"><see cref="IBonusPoolService"/></param>
        /// <param name="logger"><see cref="ILogger"/></param>
        public BonusPoolController(IBonusPoolService bonusPoolService, ILogger logger)
        {
            _bonusPoolService = bonusPoolService;
            _logger = logger;
        }

        /// <summary>
        /// Caclulates Bonus for <see cref="CalculateBonusDto"/>
        /// </summary>
        /// <param name="request"><see cref="CalculateBonusDto"/></param>
        /// <returns><see cref="BonusPoolCalculatorResultDto"/> with employee details and bonus amount</returns>
        [HttpPost()]
        public async Task<IActionResult> CalculateBonus([FromBody] CalculateBonusDto request)
        {
            try
            {
                if (request == null || request.SelectedEmployeeId < 1)
                    return BadRequest("SelectedEmployeeId is not a valid Positive Integer. Please try again with valid SelectedEmployeeId.");

                var bonusPoolResult = await _bonusPoolService.CalculateAsync(
                    request.TotalBonusPoolAmount,
                    request.SelectedEmployeeId);

                if (bonusPoolResult == null)
                    return BadRequest($"Invalid SelectedEmployeeId : {request.SelectedEmployeeId}, Employee doesnt exist. " +
                        $"Please try again with valid SelectedEmployeeId.");

                return Ok(bonusPoolResult);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error while Fetching employee. Exception details : {ex}");
                throw;
            }
        }
    }
}