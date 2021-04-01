namespace Jobz.WebUi.Validation.Validators
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentValidation;
    using Models;

    public class WaterEventCreateModelValidator : AbstractValidator<WaterEventCreateModel>
    {
        private List<string> _validDays = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        private List<string> _validZones = new List<string> { "NorthGrass", "SouthGrass", "Trees", "Bushes" };
        public WaterEventCreateModelValidator()
        {
            RuleFor(x => x.ZoneId).Must(VeValidZoneId).WithMessage(r => $"ZoneName is not valid. ZoneName:{r.ZoneId}");
            RuleFor(x => x.DayOfWeek).Must(BeValidDayOfWeek).WithMessage(r => $"Day of Week is not valid. DayOfWeek:{r.DayOfWeek}");
            RuleFor(x => x.DurationMinutes).GreaterThan(0).WithMessage(r => $"Duration minutes should be greater than 0. Duration:{r.DurationMinutes}"); ;
        }

        public bool BeValidDayOfWeek(string dayOfWeek)
        {
            return _validDays.Any(d => d == dayOfWeek);
        }

        public bool VeValidZoneId(string zoneId)
        {
            return _validZones.Any(d => d == zoneId);
        }
    }
}