namespace Jobz.WebUi.Validation.Validators
{
    using System;
    using FluentValidation;
    using Models;

    public class RainDelayUpdateModelValidator : AbstractValidator<RainDelayUpdateModel>
    {
        public RainDelayUpdateModelValidator()
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.RainDelayExpire)
                .Must(BeFutureIfNotNull)
                .WithMessage(x => $"Rain Delay expire must be in the future when not null.");
        }

        private bool BeFutureIfNotNull(DateTime? rainDelayExpire)
        {
            var result = rainDelayExpire == null
                         || rainDelayExpire.Value >= DateTime.Now;
            return result;
        }
    }
}