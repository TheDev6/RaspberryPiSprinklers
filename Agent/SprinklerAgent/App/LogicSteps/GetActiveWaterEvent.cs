namespace SprinklerAgent.App.LogicSteps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Configuration;
    using DeadSimpleCache;
    using Logging;
    using LogicLadder;
    using Microsoft.ApplicationInsights.DataContracts;
    using Models;
    using Sprinkler_Api.Models;
    using Weather;

    public class GetActiveWaterEvent : IStep<SprinklerAgentContext>
    {
        private readonly IAppLogger _logger;
        private readonly ISprinklerManager _sprinklerManager;
        private readonly IWeatherApiClient _weatherApiClient;
        private readonly IConfig _config;
        private readonly SimpleCache _cache;

        public GetActiveWaterEvent(
            IAppLogger logger,
            ISprinklerManager sprinklerManager,
            IWeatherApiClient weatherApiClient,
            IConfig config, SimpleCache cache)
        {
            _logger = logger;
            _sprinklerManager = sprinklerManager;
            _weatherApiClient = weatherApiClient;
            _config = config;
            _cache = cache;
        }

        public bool ContinueOnError => false;
        public async Task<SprinklerAgentContext> RunStep(SprinklerAgentContext context)
        {
            if (context.Schedule?.WaterEvents?.Count > 0)
            {
                var activeEvent = GetActiveEvent(context?.Schedule?.WaterEvents);
                if (activeEvent != null)
                {
                    context.ActiveWaterEvent = activeEvent;
                }
            }
            return context;
        }

        public WeeklyWaterEvent GetActiveEvent(List<WeeklyWaterEvent> waterEvents)
        {
            WeeklyWaterEvent result = null;
            if (waterEvents?.Any() == true)
            {
                var nowSpan = DateTime.Now.TimeOfDay;
                var events = waterEvents
                    .Where(evt => !string.IsNullOrWhiteSpace(evt.ZoneName)
                                  && evt.DayOfWeek == DateTime.Now.DayOfWeek
                                  && nowSpan >= evt.Start
                                  && nowSpan <= evt.End).ToList();
                if (events.Count == 1)
                {
                    result = events[0];
                }
                else if (events.Count > 1)
                {
                    _logger.Log(
                        message: "RunSprinklers.RunStep_Found_Overlapping_Water_Events",
                        level: SeverityLevel.Warning,
                        properties: new Dictionary<string, string>
                        {
                            {LogLabels.EventCount, waterEvents.Count.ToString()},
                            {LogLabels.EventNames, string.Join(",", waterEvents.Select(e => e.ZoneName))},
                            {LogLabels.RunningZone, _sprinklerManager.GetActiveZone()?.ZoneId ?? "null"}
                        });
                    result = events.Take(1).ToList()[0];//arbitrarily pick one. Let linq do it. I have no good rule for the winner.
                }
            }
            return result;
        }
    }
}
