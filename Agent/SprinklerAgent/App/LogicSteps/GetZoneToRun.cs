namespace SprinklerAgent.App.LogicSteps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Logging;
    using LogicLadder;
    using Microsoft.ApplicationInsights.DataContracts;
    using Models;

    public class GetZoneToRun : IStep<SprinklerAgentContext>
    {
        private readonly ISprinklerManager _sprinklerManager;
        private readonly IAppLogger _logger;

        public GetZoneToRun(
            IAppLogger logger,
            ISprinklerManager sprinklerManager)
        {
            _logger = logger;
            _sprinklerManager = sprinklerManager;
        }

        public bool ContinueOnError => false;
        public Task<SprinklerAgentContext> RunStep(SprinklerAgentContext context)
        {
            if (context.ActiveWaterEvent != null)
            {
                var zone = SprinklerZones.AsList().SingleOrDefault(z => z.ZoneId == context.ActiveWaterEvent.ZoneName);
                switch (context.ActiveWaterEvent.ZoneName)
                {
                    case ZoneLabels.NorthGrass:
                        zone = SprinklerZones.NorthGrass;
                        break;
                    case ZoneLabels.SouthGrass:
                        zone = SprinklerZones.SouthGrass;
                        break;
                    case ZoneLabels.Trees:
                        zone = SprinklerZones.Trees;
                        break;
                    case ZoneLabels.Bushes:
                        zone = SprinklerZones.Bushes;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"ZoneName:{context.ActiveWaterEvent.ZoneName} is not recognized.");
                }

                var activeZone = _sprinklerManager.GetActiveZone();
                if (activeZone == null)//nothing running,so run this zone
                {
                    _logger.Log(
                        message: "GetZoneToRun.RunStep_FoundZoneToRun",
                        level: SeverityLevel.Information,
                        properties: new Dictionary<string, string>
                        {
                            {LogLabels.RunningZone, zone?.ZoneId},
                            {LogLabels.ActiveEvent, context?.ActiveWaterEvent?.ZoneName}
                        });
                    context.ZoneToRun = zone;
                }
                else if (activeZone?.ZoneId == zone.ZoneId)//this zone is already running, all good.
                {
                    _logger.Log(
                        message: "GetZoneToRun.RunStep_Active_Water_Event_Running",
                        level: SeverityLevel.Information,
                        properties: new Dictionary<string, string>
                        {
                            {LogLabels.RunningZone, zone?.ZoneId},
                            {LogLabels.ActiveEvent, context?.ActiveWaterEvent?.ZoneName}
                        });
                }
                else if (activeZone?.ZoneId != null
                        && activeZone.ZoneId != zone.ZoneId)//Another zone is running, wait, log
                {
                    _logger.Log(
                        message: "GetZoneToRun.RunStep_ActiveWaterEvent_WaitingForZoneToFinish",
                        level: SeverityLevel.Information,
                        properties: new Dictionary<string, string>
                        {
                            {LogLabels.RunningZone, zone?.ZoneId},
                            {LogLabels.ActiveEvent, context?.ActiveWaterEvent?.ZoneName}
                        });
                }
            }

            return Task.FromResult(context);
        }
    }
}
