using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HttpClients.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelsDto;

namespace UpdateValueService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private readonly int _maxValue;

        private readonly int _minValue;

        private readonly int _timer;

        private readonly ITowerService _towerService;


        public Worker(IConfiguration configuration,
            ILogger<Worker> logger, ITowerService service
        )
        {
            _logger = logger;
            _timer = GetTimer(configuration);
            _minValue = GetMinValue(configuration);
            _maxValue = GetMaxValue(configuration);
            _towerService = service;
        }


        private int GetTimer(IConfiguration config)
        {
            if (!int.TryParse(config.GetSection("Timer").Value, out int result))
            {
                result = 60000;
            }

            return result;
        }

        private int GetMinValue(IConfiguration config)
        {
            if (!int.TryParse(config.GetSection("MinValue").Value, out int result))
            {
                result = 0;
            }

            return result;
        }

        private int GetMaxValue(IConfiguration config)
        {
            if (!int.TryParse(config.GetSection("MaxValue").Value, out int result))
            {
                result = 100;
            }

            return result;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var tower = await _towerService.GetAllTowerAsync();


                    if (tower is ObjectResult towerObjectResult)
                    {
                        if (towerObjectResult.StatusCode != 200)
                        {
                            _logger.LogInformation(
                                $"Bad answer form server {towerObjectResult.Value} [{towerObjectResult.StatusCode}]");
                        }
                        else
                        {
                            if (towerObjectResult.Value is IEnumerable<IndicatorDto> towerJson)
                            {
                                _logger.LogInformation($"Good answer from server [{towerObjectResult.StatusCode}]",
                                    DateTimeOffset.Now);

                                var towerJsonModels = towerJson as IndicatorDto[] ?? towerJson.ToArray();

                                if (towerJsonModels.Any())
                                {
                                    await UpdateValueToServer(towerJsonModels);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"{ex.Message}", DateTimeOffset.Now);
                }

                await Task.Delay(_timer, stoppingToken);
            }
        }


        private async Task UpdateValueToServer(IEnumerable<IndicatorDto> towers)
        {
            var towerModels = towers.ToList();
            foreach (var tower in towerModels)
            {
                Random valueRandom = new Random();
                tower.value = valueRandom.Next(_minValue, _maxValue);
                tower.dateValue = DateTime.Now.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                _logger.LogInformation(
                    $"Tower Id:{tower.id} Name:{tower.title} Min:{tower.minValue} Max: {tower.maxValue} Value:{tower.value}");
            }

            var result = await _towerService.UpdateTowersAsync(towerModels);

            if (result is ObjectResult statusResult)
            {
                if (statusResult.StatusCode != 200)
                {
                    _logger.LogInformation(
                        $"Bad answer form server {statusResult.Value} [{statusResult.StatusCode}]");
                }
                else
                {
                    _logger.LogInformation($"Good answer from server [{statusResult.StatusCode}]", DateTimeOffset.Now);
                }
            }
        }
    }
}