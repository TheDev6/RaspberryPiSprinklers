namespace SprinklerApi.AppLogic
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Data.Tables;
    using Models;
    using Security;
    using Validators;

    public class AppUserLogic
    {
        private readonly ISprinklerDataClient _dataClient;
        private readonly HashUtility _hashUtil;
        private readonly AppUser_CreateValidator _createValidator;

        public AppUserLogic(
            ISprinklerDataClient dataClient,
            HashUtility hashUtil,
            AppUser_CreateValidator createValidator)
        {
            _dataClient = dataClient;
            _hashUtil = hashUtil;
            _createValidator = createValidator;
        }

        public async Task<StandardResponse<AppUserModel>> CreateAppUser(AppUserCreateModel model)
        {
            var result = new StandardResponse<AppUserModel>();
            var validationResult = await _createValidator.ValidateAsync(model);
            result.ValidationMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            if (validationResult.IsValid)
            {
                var user = new AppUser();
                user.Username = model.Username;
                user.UserUid = Guid.NewGuid();
                user.Salt = _hashUtil.GenerateSalt();
                user.PasswordBase64Hash = _hashUtil.Hash(toBeHashed: model.Password, salt: user.Salt);
                user.CommaSeparatedClaims = string.Join(',', model.Claims);
                await _dataClient.CallAsync(async db =>
                {
                    db.AppUsers.Add(user);
                    await db.SaveChangesAsync();
                });
                result.Payload = new AppUserModel()
                {
                    Username = user.Username,
                    Claims = user?.CommaSeparatedClaims?.Split(',').ToList(),
                    UserUid = user.UserUid
                };
            }
            return result;
        }
    }
}
