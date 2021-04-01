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

    public class WeeklyWaterEventLogic
    {
        private readonly ISprinklerDataClient _dataClient;
        private readonly WeeklyWaterEvent_UpdateValidator _updateValidator;
        private readonly WeeklyWaterEvent_CreateValidator _createValidator;

        public WeeklyWaterEventLogic(
            ISprinklerDataClient dataClient,
            WeeklyWaterEvent_UpdateValidator updateValidator,
            WeeklyWaterEvent_CreateValidator createValidator)
        {
            _dataClient = dataClient;
            _updateValidator = updateValidator;
            _createValidator = createValidator;
        }

        public async Task<StandardResponse<WeeklyWaterEvent>> Update(WeeklyWaterEventUpdateModel updateModel)
        {
            var result = new StandardResponse<WeeklyWaterEvent>();
            var validationResult = await _updateValidator.ValidateAsync(updateModel);
            result.ValidationMessages = validationResult.Errors.Select(r => r.ErrorMessage)?.ToList() ?? new List<string>();
            if (validationResult.IsValid)
            {
                await _dataClient.CallAsync(async db =>
                {
                    var we = await db.WeeklyWaterEvents.SingleOrDefaultAsync(w =>
                        w.WeeklyWaterEventUid == updateModel.WeeklyWaterEventUid);
                    Enum.TryParse(updateModel.DayOfWeek, true, out DayOfWeek dow);
                    we.DayOfWeek = dow;
                    we.Start = new TimeSpan(hours: updateModel.StartHour, minutes: updateModel.StartMinute, seconds: updateModel.StartSecond);
                    we.End = we.Start.Add(new TimeSpan(hours: 0, minutes: updateModel.DurationMinutes, seconds: updateModel.DurationSeconds));
                    we.ZoneName = updateModel.ZoneName;
                    await db.SaveChangesAsync();
                    result.Payload = we;
                });
            }

            return result;
        }

        public async Task<StandardResponse<WeeklyWaterEvent>> Create(WeeklyWaterEventCreateModel createModel)
        {
            var result = new StandardResponse<WeeklyWaterEvent>();
            var validationResult = await _createValidator.ValidateAsync(createModel);
            result.ValidationMessages = validationResult.Errors.Select(r => r.ErrorMessage)?.ToList() ?? new List<string>();
            if (validationResult.IsValid)
            {
                await _dataClient.CallAsync(async db =>
                {
                    var we = new WeeklyWaterEvent();
                    Enum.TryParse(createModel.DayOfWeek, true, out DayOfWeek dow);
                    we.DayOfWeek = dow;
                    we.Start = new TimeSpan(hours: createModel.StartHour, minutes: createModel.StartMinute, seconds: createModel.StartSecond);
                    we.End = we.Start.Add(new TimeSpan(hours: 0, minutes: createModel.DurationMinutes, seconds: createModel.DurationSeconds));
                    we.ZoneName = createModel.ZoneName;
                    we.WeeklyWaterEventUid = Guid.NewGuid();
                    await db.WeeklyWaterEvents.AddAsync(we);
                    await db.SaveChangesAsync();
                    result.Payload = we;
                });
            }

            return result;
        }

        public async Task<StandardResponse<Guid>> Delete(Guid weeklyWaterEventGuid)
        {
            var result = new StandardResponse<Guid>();

            await _dataClient.CallAsync(async db =>
            {
                var we = await db.WeeklyWaterEvents.SingleOrDefaultAsync(w =>
                    w.WeeklyWaterEventUid == weeklyWaterEventGuid);
                if (we != null)
                {
                    db.WeeklyWaterEvents.Remove(we);
                    await db.SaveChangesAsync();
                    result.Payload = weeklyWaterEventGuid;
                }
                else
                {
                    result.ValidationMessages.Add($"No record found to delete WeeklyWaterEventGuid:{weeklyWaterEventGuid}");
                }
            });

            return result;
        }

        public async Task<StandardResponse<WeeklyWaterEvent>> Get(Guid weeklyWaterEventUid)
        {
            var result = new StandardResponse<WeeklyWaterEvent>();
            result.Payload = await _dataClient.CallAsync(db =>
                db.WeeklyWaterEvents.SingleOrDefaultAsync(r =>
                    r.WeeklyWaterEventUid == weeklyWaterEventUid));

            return result;
        }

        public async Task<StandardResponse<List<WeeklyWaterEvent>>> GetAll()
        {
            var result = new StandardResponse<List<WeeklyWaterEvent>>();
            result.Payload = await _dataClient.CallAsync(db => db.WeeklyWaterEvents.ToListAsync());
            return result;
        }

        public async Task<StandardResponse<ScheduleModel>> GetSchedule()
        {
            var result = new StandardResponse<ScheduleModel>();
            var scheduleModel = new ScheduleModel();
            scheduleModel.WaterEvents = await _dataClient.CallAsync(db => db.WeeklyWaterEvents.ToListAsync());
            scheduleModel.RainDelay = await _dataClient.CallAsync(db =>
                db.RainDelays.SingleOrDefaultAsync(r => r.RainDelayExpireDate > DateTime.Now));
            result.Payload = scheduleModel;
            return result;
        }
    }
}

