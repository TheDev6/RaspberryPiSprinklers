namespace SprinklerApi.AppLogic.Validators
{
    using System.Linq;
    using Data;
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class WaterEventHistory_UpdateValidator : AbstractValidator<WaterEventHistoryUpdateModel>
    {
        private readonly ISprinklerDataClient _dataClient;
        public WaterEventHistory_UpdateValidator(ISprinklerDataClient dataClient)
        {
            _dataClient = dataClient;

            RuleFor(x => x.WaterEventHistoryUid)
                .NotEmpty()
                .MustAsync(async (model, guid, token) =>
                    await _dataClient.CallAsync(db =>
                        db.WaterEventHistories.AnyAsync(h => h.WaterEventHistoryUid == guid)))
                .WithMessage((model, prop) => $"No History record exists with id:'{prop}'");
            RuleFor(x => x.ZoneName)
                .Must(r => ValidationConstants.ValidZones.Any(d => d == r))
                .WithMessage(r => $"ZoneName:{r.ZoneName} is not valid.");
            RuleFor(x => x.Start).NotEmpty();
            RuleFor(x => x.Message).NotEmpty();
        }
    }
}
