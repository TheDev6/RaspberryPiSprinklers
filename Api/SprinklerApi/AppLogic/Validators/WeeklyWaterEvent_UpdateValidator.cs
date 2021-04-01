namespace SprinklerApi.AppLogic.Validators
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Data;
    using Data.Tables;
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class WeeklyWaterEvent_UpdateValidator : AbstractValidator<WeeklyWaterEventUpdateModel>
    {
        private readonly ISprinklerDataClient _dataClient;
        public WeeklyWaterEvent_UpdateValidator(ISprinklerDataClient dataClient)
        {
            _dataClient = dataClient;

            RuleFor(x => x).MustAsync(ExistsForUpdate).WithMessage(r => $"{r.WeeklyWaterEventUid} does not exist for updating.");
            RuleFor(x => x).MustAsync(NotOverlap).WithMessage(r => $"{r.WeeklyWaterEventUid} overlaps with another water event.");
            RuleFor(x => x.ZoneName).Must(BeValidZoneId).WithMessage(r => $"ZoneName:'{r.ZoneName}' is not recognized.");
            RuleFor(x => x.DayOfWeek).Must(BeValidDayOfWeek).WithMessage(r => $"Day of Week is not valid. DayOfWeek:{r.DayOfWeek}");
            RuleFor(x => x.DurationMinutes).GreaterThan(0).WithMessage(r => $"Duration minutes should be greater than 0. Duration:{r.DurationMinutes}");
        }

        public bool BeValidDayOfWeek(string dayOfWeek)
        {
            return ValidationConstants.ValidDays.Any(d => d == dayOfWeek);
        }

        public bool BeValidZoneId(string zoneId)
        {
            return ValidationConstants.ValidZones.Any(d => d == zoneId);
        }

        public async Task<bool> ExistsForUpdate(WeeklyWaterEventUpdateModel model, CancellationToken token)
        {
            return await _dataClient.CallAsync(async db =>
                await db.WeeklyWaterEvents
                    .AnyAsync(r => r.WeeklyWaterEventUid == model.WeeklyWaterEventUid, token) == true);
        }

        public async Task<bool> NotOverlap(WeeklyWaterEventUpdateModel model, CancellationToken token)
        {
            var hasOverlap = false;
            var waterEvents = await _dataClient.CallAsync(ctx => ctx.WeeklyWaterEvents
                .Where(w => w.WeeklyWaterEventUid != model.WeeklyWaterEventUid)
                .ToListAsync(cancellationToken: token));
            foreach (var e in waterEvents)
            {
                if (this.IsOverlap(e, model))
                {
                    hasOverlap = true;
                    break;
                }
            }
            return hasOverlap == false;
        }

        private bool IsOverlap(WeeklyWaterEvent a, WeeklyWaterEventUpdateModel b)
        {
            var aStart = a.Start;
            Enum.TryParse(b.DayOfWeek, out DayOfWeek bDayOfWeek);
            var bStart = new TimeSpan(hours: b.StartHour, minutes: b.StartMinute, seconds: b.StartSecond);
            var bEnd = new TimeSpan(hours: b.StartHour, minutes: b.DurationMinutes, seconds: b.DurationSeconds);

            var result = aStart >= bStart
                         && aStart <= bEnd
                         && a.DayOfWeek == bDayOfWeek;
            return result;
        }
    }
}
