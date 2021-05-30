using System.Collections.Generic;
using System.Threading.Tasks;
using HttpClients.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ModelsDto;

namespace HttpClients
{
    public class TowerClient : BaseClient, ITowerService
    {
        public TowerClient(string baseAddress) : base(baseAddress)
        {
            ServiceAddress = "api/Indicator";
        }

        protected override string ServiceAddress { get; }


        public async Task<IActionResult> GetTowerAsync(string id)
        {
            return await GetAsync<IndicatorDto>($"{ServiceAddress}/{id}");
        }

        public async Task<IActionResult> GetAllTowerAsync()
        {
            return await GetAsync<IEnumerable<IndicatorDto>>($"{ServiceAddress}/all");
        }

        public async Task<IActionResult> UpdateTowerAsync(IndicatorDto tower)
        {
            return await PostAsync($"{ServiceAddress}/update", tower);
        }


        public async Task<IActionResult> UpdateTowersAsync(IEnumerable<IndicatorDto> towers)
        {
            return await PostAsync($"{ServiceAddress}/updates", towers);
        }

        public async Task<IActionResult> NewTowersAsync(IndicatorDto tower)
        {
            return await PostAsync($"{ServiceAddress}/new", tower);
        }

        public async Task<IActionResult> DeleteTowersAsync(IndicatorDto tower)
        {
            return await PostAsync($"{ServiceAddress}/delete", tower);
        }
    }
}