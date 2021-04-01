namespace SprinklerApi.AppLogic
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Data.Tables;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Validators;

    public class RainDelayLogic
    {
        private readonly ISprinklerDataClient _dataClient;
        private readonly RainDelay_CreateValidator _createValidator;

        public RainDelayLogic(
            ISprinklerDataClient dataClient,
            RainDelay_CreateValidator createValidator)
        {
            _dataClient = dataClient;
            _createValidator = createValidator;
        }

        public async Task<StandardResponse<RainDelay>> GetActiveRainDelay()
        {
            var result = new StandardResponse<RainDelay>();
            result.Payload = await _dataClient.CallAsync(db => db.RainDelays.SingleOrDefaultAsync(r => r.RainDelayExpireDate > DateTime.Now));
            return result;
        }

        public async Task<StandardResponse<int>> DeleteExpiredRainDelays()
        {
            var result = new StandardResponse<int>();
            await _dataClient.CallAsync(async db =>

            {
                var rd = await db.RainDelays.Where(r => r.RainDelayExpireDate < DateTime.Now).ToListAsync();
                result.Payload = rd.Count;
                if (rd.Any())
                {
                    db.RemoveRange(rd);
                    await db.SaveChangesAsync();
                }
            });
            return result;
        }

        public async Task<StandardResponse<RainDelay>> CreateRainDelay(RainDelayCreateModel model)
        {
            var result = new StandardResponse<RainDelay>();
            var validationResult = await _createValidator.ValidateAsync(model);
            result.ValidationMessages.AddRange(validationResult.Errors.Select(x => x.ErrorMessage));
            if (validationResult.IsValid)
            {
                result.Payload = await _dataClient.CallAsync(async db =>
                {
                    var rd = new RainDelay();
                    rd.RainDelayExpireDate = model.RainDelayExpireDateTime;
                    rd.RainDelayUid = Guid.NewGuid();
                    await db.RainDelays.AddAsync(rd);
                    await db.SaveChangesAsync();
                    return rd;
                });
            }
            return result;
        }

        public async Task<StandardResponse<int>> Delete(Guid rainDelayUid)
        {
            var result = new StandardResponse<int>();
            result.Payload = await _dataClient.CallAsync(async db =>
            {
                var lamResult = 0;
                var rd = await db.RainDelays.SingleOrDefaultAsync(w => w.RainDelayUid == rainDelayUid);
                if (rd != null)
                {
                    db.RainDelays.RemoveRange(rd);
                    await db.SaveChangesAsync();
                    lamResult = 1;
                }
                return lamResult;
            });
            return result;
        }
    }
}
