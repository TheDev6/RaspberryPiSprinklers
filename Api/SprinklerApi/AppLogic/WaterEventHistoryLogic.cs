namespace SprinklerApi.AppLogic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Data.Tables;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Validators;

    public class WaterEventHistoryLogic
    {
        private readonly ISprinklerDataClient _dataClient;
        private readonly WaterEventHistory_CreateValidator _createValidator;
        private readonly WaterEventHistory_UpdateValidator _updateValidator;

        public WaterEventHistoryLogic(
            ISprinklerDataClient dataClient,
            WaterEventHistory_CreateValidator createValidator,
            WaterEventHistory_UpdateValidator updateValidator)
        {
            _dataClient = dataClient;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<StandardResponse<List<WaterEventHistory>>> GetByDateRange(DateTime start, DateTime end)
        {
            var result = new StandardResponse<List<WaterEventHistory>>();
            result.Payload = await _dataClient.CallAsync(db => db.WaterEventHistories
                .Where(w =>
                    w.Start >= start
                    && w.Start <= end)
                .OrderByDescending(w => w.Start)
                .ToListAsync());

            return result;
        }

        public async Task<StandardResponse<WaterEventHistory>> Create(WaterEventHistoryCreateModel model)
        {
            var result = new StandardResponse<WaterEventHistory>();
            var validationResult = await _createValidator.ValidateAsync(model);
            result.ValidationMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            if (validationResult.IsValid)
            {
                result.Payload = await _dataClient.CallAsync(async db =>
                {
                    var wh = new WaterEventHistory();
                    wh.Start = model.Start;
                    wh.End = model.End;
                    wh.ZoneName = model.ZoneName;
                    wh.WaterEventHistoryUid = Guid.NewGuid();
                    wh.Message = model.Message;

                    db.WaterEventHistories.Add(wh);
                    await db.SaveChangesAsync();
                    return wh;
                });
            }
            return result;
        }

        public async Task<StandardResponse<WaterEventHistory>> Update(WaterEventHistoryUpdateModel model)
        {
            var result = new StandardResponse<WaterEventHistory>();
            var validationResult = await _updateValidator.ValidateAsync(model);
            result.ValidationMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            if (validationResult.IsValid)
            {
                result.Payload = await _dataClient.CallAsync(async db =>
                {
                    var wh = await db.WaterEventHistories.SingleOrDefaultAsync(x => x.WaterEventHistoryUid == model.WaterEventHistoryUid);
                    if (wh != null)
                    {
                        wh.Start = model.Start;
                        wh.End = model.End;
                        wh.ZoneName = model.ZoneName;
                        wh.Message = model.Message;
                        await db.SaveChangesAsync();
                    }
                    return wh;
                });
            }
            return result;
        }

        public async Task<StandardResponse<int>> Delete(Guid waterEventHistoryUid)
        {
            var result = new StandardResponse<int>();
            result.Payload = await _dataClient.CallAsync(async db =>
            {
                var wh = await db.WaterEventHistories.SingleOrDefaultAsync(x => x.WaterEventHistoryUid == waterEventHistoryUid);
                db.WaterEventHistories.Remove(wh);
                return await db.SaveChangesAsync();
            });
            return result;
        }
    }
}
