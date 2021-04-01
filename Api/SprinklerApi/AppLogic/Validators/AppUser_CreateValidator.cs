namespace SprinklerApi.AppLogic.Validators
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Data;
    using Data.Tables;
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class AppUser_CreateValidator : AbstractValidator<AppUserCreateModel>
    {
        private readonly ISprinklerDataClient _dataClient;
        public AppUser_CreateValidator(ISprinklerDataClient dataClient)
        {
            _dataClient = dataClient;

            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Username)
                .MustAsync(NotBeDuplicateUsername)
                .WithMessage(x => $"Username:{x.Username} is not valid, or already exists.");
            RuleFor(x => x.Claims)
                .NotEmpty()
                .Must(IsValidClaims)
                .WithMessage(r => $"One or more invalid claim value(s) detected.");
        }

        public async Task<bool> NotBeDuplicateUsername(AppUserCreateModel model, string username, CancellationToken token)
        {
            var hasDuplicate = await _dataClient.CallAsync(db => db.AppUsers.AnyAsync(u => u.Username == username, token));
            return hasDuplicate == false;
        }

        public bool IsValidClaims(AppUserCreateModel model, List<string> claims)
        {
            var isValid = true;
            foreach (var c in claims)
            {
                if (Claims.ClaimsAsList.Any(item => c == item) == false)
                {
                    isValid = false;
                }
            }
            return isValid;
        }
    }
}
