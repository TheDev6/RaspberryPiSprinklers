namespace SprinklerAgent.App.LogicSteps
{
    using System.IO;
    using System.Threading.Tasks;
    using DeadSimpleCache;
    using LogicLadder;
    using Models;
    using Newtonsoft.Json;
    using Sprinkler_Api.Models;

    public class GetOfflineSchedule : IStep<SprinklerAgentContext>
    {
        private readonly SimpleCache _cache;

        public GetOfflineSchedule(SimpleCache cache)
        {
            _cache = cache;
        }

        public bool ContinueOnError => false;
        public async Task<SprinklerAgentContext> RunStep(SprinklerAgentContext context)
        {
            if (context.Schedule == null)
            {
                var cachedSchedule = _cache.Get<ScheduleModel>(CacheKeys.Schedule);
                if (!cachedSchedule.IsCacheNull)
                {
                    context.Schedule = cachedSchedule.ValueOrDefault;
                }

                if (context.Schedule == null
                    && File.Exists(context.ScheduleJsonFileName))
                {
                    var json = await File.ReadAllTextAsync(context.ScheduleJsonFileName);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        var schedule = JsonConvert.DeserializeObject<ScheduleModel>(json);
                        context.Schedule = schedule;
                    }
                }
            }

            return context;
        }
    }
}
