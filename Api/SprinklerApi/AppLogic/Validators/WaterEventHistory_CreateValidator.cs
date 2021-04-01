namespace SprinklerApi.AppLogic.Validators
{
    using System.Linq;
    using FluentValidation;
    using Models;

    public class WaterEventHistory_CreateValidator : AbstractValidator<WaterEventHistoryCreateModel>
    {
        public WaterEventHistory_CreateValidator()
        {
            RuleFor(x => x.ZoneName)
                .Must(r => ValidationConstants.ValidZones.Any(d => d == r))
                .WithMessage(r => $"ZoneName:{r.ZoneName} is not valid.");
            RuleFor(x => x.Start).NotEmpty();
            RuleFor(x => x.Message).NotEmpty();
        }
    }
}
