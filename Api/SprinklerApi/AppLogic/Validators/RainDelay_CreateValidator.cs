namespace SprinklerApi.AppLogic.Validators
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Data;
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class RainDelay_CreateValidator : AbstractValidator<RainDelayCreateModel>
    {
        private readonly ISprinklerDataClient _dataClient;

        public RainDelay_CreateValidator(ISprinklerDataClient dataClient)
        {
            _dataClient = dataClient;

            RuleFor(x => x.RainDelayExpireDateTime)
                .Must((model, time) => time > DateTime.Now)
                .WithMessage(r => $"RainDelayExpireDateTime must be in the future and is not. Val:{r.RainDelayExpireDateTime}");
            RuleFor(x => x).MustAsync(NotHaveActiveRainDelay).WithMessage(r => $"There is already an active rain delay.");
        }

        public async Task<bool> NotHaveActiveRainDelay(RainDelayCreateModel model, CancellationToken token)
        {
            var hasActiveRainDelay = await _dataClient.CallAsync(db => db.RainDelays
                .AnyAsync(r => r.RainDelayExpireDate > DateTime.Now, cancellationToken: token));
            return hasActiveRainDelay == false;
        }
    }
}
