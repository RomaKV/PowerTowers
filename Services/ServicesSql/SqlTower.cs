using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonHelper;
using EntitySql.Entity;
using HttpClients.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsDto;

namespace SqlServices
{
    public class SqlTower : ControllerBase, ITowerService
    {
        private readonly TowerContext _context;

        public SqlTower(TowerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetTowerAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new ObjectResult(
                        "Tower Id didn't set")
                    {StatusCode = 400};
            }

            if (!int.TryParse(id, out var TowerId))
            {
                return new ObjectResult(
                        $"Decoding Tower Id [{id}] failed")
                    {StatusCode = 400};
            }

            try
            {
                var value = await _context.TowerValues.AsNoTracking()
                    .Include(t => t.Tower)
                    .Join(_context.TowerValues.GroupBy(v => v.TowerId)
                            .Select(g => new {TowerId = g.Key, Date = g.Max(d => d.Date)}),
                        v => new {v.TowerId, v.Date}, vm => new {vm.TowerId, vm.Date}, (v, vm) => new IndicatorDto
                        {
                            id = v.TowerId.ToString(),
                            title = v.Tower.Name,
                            minValue = v.Tower.MinValue,
                            maxValue = v.Tower.MaxValue,
                            value = v.Value,
                            dateValue = v.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")
                        }).SingleOrDefaultAsync(val => val.id == TowerId.ToString());

                if (value != null)
                {
                    return new ObjectResult(value) {StatusCode = 200};
                }
            }
            catch (Exception exc)
            {
                return new ObjectResult(
                        $"{HelperMsg.GetAllMessages(exc, $"Error has occurred in {GetType().FullName} GetTowerAsync(id)")}")
                    {StatusCode = 500};
            }


            return new ObjectResult(
                    $"Tower did not find, which has a tower Id [{TowerId}]")
                {StatusCode = 400};
        }


        public async Task<IActionResult> GetAllTowerAsync()
        {
            List<IndicatorDto> result = new List<IndicatorDto>();

            try
            {
                var valueTowers = await _context.TowerValues.AsNoTracking()
                    .Include(t => t.Tower)
                    .Join(_context.TowerValues.GroupBy(v => v.TowerId)
                            .Select(g => new {TowerId = g.Key, Date = g.Max(d => d.Date)}),
                        v => new {v.TowerId, v.Date}, vm => new {vm.TowerId, vm.Date}, (v, vm) => new IndicatorDto
                        {
                            id = v.TowerId.ToString(),
                            title = v.Tower.Name,
                            minValue = v.Tower.MinValue,
                            maxValue = v.Tower.MaxValue,
                            value = v.Value,
                            dateValue = v.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")
                        }).ToListAsync();

                if (valueTowers?.Any() == true)
                {
                    result = valueTowers;
                }
            }
            catch (Exception exc)
            {
                return new ObjectResult(
                        $"{HelperMsg.GetAllMessages(exc, $"Error has occurred in {GetType().FullName} GetAllTowerAsync")}")
                    {StatusCode = 500};
            }

            return new ObjectResult(result) {StatusCode = 200};
        }

        public async Task<IActionResult> UpdateTowerAsync(IndicatorDto tower)
        {
            if (tower == null)
            {
                return new ObjectResult(
                        "Tower for updating did not find [tower = null]")
                    {StatusCode = 400};
            }

            if (!int.TryParse(tower.id, out var idTower))
            {
                return new ObjectResult(
                        $"Decoding Tower Id [{tower.id}] failed")
                    {StatusCode = 400};
            }


            try
            {
                var towerUpdate = await _context.Towers
                    .SingleOrDefaultAsync(i => i.Id == idTower);

                if (towerUpdate != null)
                {
                    if (UpdateTower(towerUpdate, tower))
                    {
                        await _context.SingleUpdateAsync(towerUpdate);
                    }

                    var value = InsertTowerValue(idTower, tower);

                    if (value != null)
                    {
                        await _context.TowerValues.AddAsync(value);
                        await _context.SaveChangesAsync();
                    }


                    var loadUpdated = await _context.TowerValues.AsNoTracking()
                        .Include(t => t.Tower)
                        .Join(_context.TowerValues.GroupBy(v => v.TowerId)
                                .Select(g => new {TowerId = g.Key, Date = g.Max(d => d.Date)}),
                            v => new {v.TowerId, v.Date}, vm => new {vm.TowerId, vm.Date}, (v, vm) => new IndicatorDto
                            {
                                id = v.TowerId.ToString(),
                                title = v.Tower.Name,
                                minValue = v.Tower.MinValue,
                                maxValue = v.Tower.MaxValue,
                                value = v.Value,
                                dateValue = v.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")
                            }).SingleOrDefaultAsync(val => val.id == idTower.ToString());

                    if (loadUpdated != null)
                    {
                        return new ObjectResult(loadUpdated) {StatusCode = 200};
                    }
                }

                return new ObjectResult(
                        $"Tower for updating did not find which has a tower Id [{idTower}]")
                    {StatusCode = 400};
            }
            catch (Exception exc)
            {
                return new ObjectResult(
                        $"{HelperMsg.GetAllMessages(exc, $"Error has occurred in {GetType().FullName} GetAllTowerAsync")}")
                    {StatusCode = 500};
            }
        }

        public async Task<IActionResult> UpdateTowersAsync(IEnumerable<IndicatorDto> towers)
        {
            if (towers == null)
            {
                return new ObjectResult(
                        "Towers for updating did not find [towers = null]")
                    {StatusCode = 400};
            }

            try
            {
                var towerModels = towers as IndicatorDto[] ?? towers.ToArray();

                int[] ids = towerModels.Select(t => int.Parse(t.id)).ToArray();

                if (ids.Length > 0)
                {
                    var towerUpdate = await _context.Towers.Where(t => ids.Contains(t.Id)).ToListAsync();

                    if (towerUpdate?.Any() == true)
                    {
                        List<TowerValue> values = new List<TowerValue>();
                        bool updated = false;
                        foreach (var tower in towerUpdate)
                        {
                            var towerModel = towerModels.SingleOrDefault(i => i.id == tower.Id.ToString());
                            updated = UpdateTower(tower, towerModel);

                            var value = InsertTowerValue(tower.Id, towerModel);

                            if (value != null)
                            {
                                values.Add(value);
                            }
                        }

                        if (updated)
                        {
                            await _context.BulkUpdateAsync(towerUpdate);
                        }

                        if (values.Count > 0)
                        {
                            await _context.TowerValues.AddRangeAsync(values);
                            await _context.SaveChangesAsync();
                        }

                        return new ObjectResult(
                                "Towers was updated")
                            {StatusCode = 200};
                    }
                }
            }
            catch (Exception exc)
            {
                return new ObjectResult(
                        $"{HelperMsg.GetAllMessages(exc, $"Error has occurred in {GetType().FullName} GetAllTowerAsync")}")
                    {StatusCode = 500};
            }

            return new ObjectResult(
                    "Towers did not require updating")
                {StatusCode = 200};
        }

        public async Task<IActionResult> NewTowersAsync(IndicatorDto tower)
        {
            if (tower == null)
            {
                return new ObjectResult(
                        "Tower for updating did not find [tower = null]")
                    {StatusCode = 400};
            }


            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Tower towerNew = new Tower
                    {
                        MaxValue = tower.maxValue,
                        MinValue = tower.minValue,
                        Name = tower.title
                    };

                    await _context.Towers.AddAsync(towerNew);


                    if (await _context.SaveChangesAsync() > 0)
                    {
                        if (towerNew.Id > 0)
                        {
                            await _context.TowerValues.AddAsync(new TowerValue
                                {TowerId = towerNew.Id, Value = 0, Date = DateTime.Now.ToUniversalTime()});


                            await _context.SaveChangesAsync();
                            await transaction.CommitAsync();

                            return new ObjectResult(tower) {StatusCode = 200};
                        }
                    }

                    await transaction.RollbackAsync();
                    return new ObjectResult(
                            "Tower was not created")
                        {StatusCode = 500};
                }
                catch (Exception exc)
                {
                    await transaction.RollbackAsync();

                    return new ObjectResult(
                            $"{HelperMsg.GetAllMessages(exc, $"Error has occurred in {GetType().FullName} GetAllTowerAsync")}")
                        {StatusCode = 500};
                }
            }
        }

        public async Task<IActionResult> DeleteTowersAsync(IndicatorDto tower)
        {
            if (tower == null)
            {
                return new ObjectResult(
                        "Tower for updating did not find [tower = null]")
                    {StatusCode = 400};
            }


            if (string.IsNullOrEmpty(tower.id))
            {
                return new ObjectResult(
                        "Tower Id didn't set")
                    {StatusCode = 400};
            }

            if (!int.TryParse(tower.id, out var TowerId))
            {
                return new ObjectResult(
                        $"Decoding Tower Id [{tower.id}] failed")
                    {StatusCode = 400};
            }


            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var towerDel = await _context.Towers.SingleAsync(i => i.Id == TowerId);


                    if (towerDel != null)
                    {
                        var values = await _context.TowerValues.Where(t => t.TowerId == towerDel.Id).ToListAsync();

                        if (values != null)
                        {
                            _context.TowerValues.RemoveRange(values);
                        }


                        _context.Towers.Remove(towerDel);

                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();

                        return new ObjectResult("Deleted") {StatusCode = 200};
                    }
                }
                catch (Exception exc)
                {
                    await transaction.RollbackAsync();
                    return new ObjectResult(
                            $"{HelperMsg.GetAllMessages(exc, $"Error has occurred in {GetType().FullName} GetTowerAsync(id)")}")
                        {StatusCode = 500};
                }
            }


            return new ObjectResult(
                    $"Tower did not find, which has a tower Id [{TowerId}]")
                {StatusCode = 400};
        }

        private bool UpdateTower(Tower tower, IndicatorDto towerModel)
        {
            if (tower == null)
            {
                return false;
            }

            bool updated = false;

            if (!string.IsNullOrEmpty(towerModel.title))
            {
                if (tower.Name != towerModel.title)
                {
                    tower.Name = towerModel.title;
                    updated = true;
                }
            }


            if (tower.MinValue != towerModel.minValue)
            {
                tower.MinValue = towerModel.minValue;
                updated = true;
            }

            if (tower.MaxValue != towerModel.maxValue)
            {
                tower.MaxValue = towerModel.maxValue;
                updated = true;
            }


            return updated;
        }

        private TowerValue InsertTowerValue(int id, IndicatorDto towerModel)
        {
            if (!string.IsNullOrEmpty(towerModel.dateValue))
            {
                return new TowerValue
                {
                    Value = towerModel.value,
                    Date = Convert.ToDateTime(towerModel.dateValue).ToUniversalTime(),
                    TowerId = id
                };
            }

            return null;
        }
    }
}