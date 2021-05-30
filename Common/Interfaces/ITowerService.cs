using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModelsDto;

namespace HttpClients.Interfaces
{
    public interface ITowerService
    {
        Task<IActionResult> GetTowerAsync(string id);
        Task<IActionResult> GetAllTowerAsync();
        Task<IActionResult> UpdateTowerAsync(IndicatorDto tower);
        Task<IActionResult> UpdateTowersAsync(IEnumerable<IndicatorDto> towers);

        Task<IActionResult> NewTowersAsync(IndicatorDto tower);

        Task<IActionResult> DeleteTowersAsync(IndicatorDto tower);
    }
}