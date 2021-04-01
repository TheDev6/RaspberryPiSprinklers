namespace Jobz.WebUi.Validation.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentValidation;
    using Models;

    public class RunScheduleUpdateValidator : AbstractValidator<ScheduleUpdateModel>
    {
        public RunScheduleUpdateValidator()
        {
            RuleForEach(x => x.WaterEvents).SetValidator(new WaterEventCreateModelValidator());
            RuleFor(x => x.WaterEvents).Must(NotHaveOverlaps).WithMessage(s => "There is an overlapping water event.");
        }

        private bool NotHaveOverlaps(ScheduleUpdateModel model, List<WaterEventCreateModel> waterEvents)
        {
            var hasOverlap = false;
            if (model.WaterEvents != null
                && model.WaterEvents.Any())
            {
                foreach (var e in model.WaterEvents)
                {
                    hasOverlap = model.WaterEvents.Any(w => this.IsOverlap(e, w));
                }
            }
            return hasOverlap == false;
        }

        private bool IsOverlap(WaterEventCreateModel modelA, WaterEventCreateModel modelB)
        {
            var aStart = new TimeSpan(hours: modelA.StartHour, minutes: modelA.StartMinute, seconds: modelA.StartSecond);
            var bStart = new TimeSpan(hours: modelB.StartHour, minutes: modelB.StartMinute, seconds: modelB.StartSecond);
            var bEnd = new TimeSpan(hours: modelB.StartHour, minutes: modelB.DurationMinutes, seconds: modelB.DurationSeconds);
            var result = aStart >= bStart
                         && aStart <= bEnd
                         && modelA.DayOfWeek == modelB.DayOfWeek
                         && modelA.LiveId != modelB.LiveId;
            return result;
        }
    }
}