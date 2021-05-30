using System.Collections.Generic;
using System.Threading.Tasks;
using HttpClients.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ModelsDto;

namespace HostingServiceDB.Controllers
{
    [Produces("application/json")]
    [Route("api/Indicator")]
    [ApiController]
    public class TowerApiController : Controller, ITowerService
    {
        private readonly ITowerService _towerService;

        public TowerApiController(ITowerService smsService)
        {
            _towerService = smsService;
        }

        [HttpGet("{id}")]
        [ActionName("Get")]
        public async Task<IActionResult> GetTowerAsync(string id)
        {
            return await _towerService.GetTowerAsync(id);
        }

        [HttpGet("all")]
        [ActionName("Get")]
        public async Task<IActionResult> GetAllTowerAsync()
        {
            return await _towerService.GetAllTowerAsync();
        }

        [HttpPost("update")]
        [ActionName("Post")]
        public async Task<IActionResult> UpdateTowerAsync(IndicatorDto tower)
        {
            return await _towerService.UpdateTowerAsync(tower);
        }

        [HttpPost("updates")]
        [ActionName("Post")]
        public async Task<IActionResult> UpdateTowersAsync(IEnumerable<IndicatorDto> towers)
        {
            return await _towerService.UpdateTowersAsync(towers);
        }


        [HttpPost("new")]
        [ActionName("Post")]
        public async Task<IActionResult> NewTowersAsync(IndicatorDto tower)
        {
            return await _towerService.NewTowersAsync(tower);
        }

        [HttpPost("delete")]
        [ActionName("Post")]
        public async Task<IActionResult> DeleteTowersAsync(IndicatorDto tower)
        {
            return await _towerService.DeleteTowersAsync(tower);
        }
    }
}