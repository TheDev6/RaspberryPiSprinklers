namespace SprinklerAgent.App.LogicSteps
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using DeadSimpleCache;
    using LogicLadder;
    using Models;
    using Newtonsoft.Json;
    using Sprinkler_Api;

    public class GetSchedule : IStep<SprinklerAgentContext>
    {
        private readonly ISprinklerApiClient _sprinklerApi;
        private readonly SimpleCache _cache;

        public GetSchedule(
            ISprinklerApiClient sprinklerApi,
            SimpleCache cache)
        {
            _sprinklerApi = sprinklerApi;
            _cache = cache;
        }

        public bool ContinueOnError => true;

        public async Task<SprinklerAgentContext> RunStep(SprinklerAgentContext context)
        {
            var waterEventResponse = await _sprinklerApi.GetSchedule();
            if (waterEventResponse.IsSuccess
                && waterEventResponse.Payload != null)
            {
                context.Schedule = waterEventResponse.Payload;
                context.IsManualRainDelay = context.Schedule?.RainDelay?.RainDelayExpireDate > DateTime.Now;
                _cache.Set(CacheKeys.Schedule, waterEventResponse.Payload);//cache it in case loss of internet
                await File.WriteAllTextAsync(context.ScheduleJsonFileName, JsonConvert.SerializeObject(waterEventResponse.Payload));//serialize in case of raz restart app restart offline etc.
            }
            return context;
        }
    }
}
