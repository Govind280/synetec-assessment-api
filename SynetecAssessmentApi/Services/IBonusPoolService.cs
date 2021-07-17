using SynetecAssessmentApi.Dtos;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Services
{
    public interface IBonusPoolService
    {
        Task<BonusPoolCalculatorResultDto> CalculateAsync(int bonusPoolAmount, int selectedEmployeeId);
    }
}
