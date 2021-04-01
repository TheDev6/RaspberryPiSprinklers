namespace SprinklerAgent.App.LogicSteps
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using LogicLadder;
    using Models;
    using Sprinkler_Api;
    using Sprinkler_Api.Models;

    public class RunSprinklers : IStep<SprinklerAgentContext>
    {
        private readonly ISprinklerManager _sprinklerManager;
        private readonly ISprinklerApiClient _sprinklerApi;

        public RunSprinklers(
            ISprinklerManager sprinklerManager,
            ISprinklerApiClient sprinklerApi)
        {
            _sprinklerManager = sprinklerManager;
            _sprinklerApi = sprinklerApi;
        }

        public bool ContinueOnError => false;
        public async Task<SprinklerAgentContext> RunStep(SprinklerAgentContext context)
        {
            if (context.ActiveWaterEvent != null
                && context?.ZoneToRun != null
                && context?.ZoneToRun?.ZoneId == context?.ActiveWaterEvent?.ZoneName
                && context?.IsManualRainDelay == false
                && context?.IsWeatherApiRainDelay == false)
            {
                var historyResponse = await _sprinklerApi.WaterEventHistoryCreate(new WaterEventHistoryCreateModel
                {
                    Message = $"{context.WeatherData?.WeatherApiResult?.Payload?.weather.FirstOrDefault()?.description}",
                    ZoneName = context.ZoneToRun.ZoneId,
                    Start = DateTime.Now,
                    End = null
                });

                _sprinklerManager.RunZone(
                    zone: context.ZoneToRun,
                    runSpan: context.ActiveWaterEvent.End - context.ActiveWaterEvent.Start);

                if (historyResponse.IsSuccess)
                {
                    await _sprinklerApi.WaterEventHistoryUpdate(new WaterEventHistoryUpdateModel
                    {
                        WaterEventHistoryUid = historyResponse.Payload.WaterEventHistoryUid,
                        Message = historyResponse.Payload.Message,
                        ZoneName = historyResponse.Payload.ZoneName,
                        Start = historyResponse.Payload.Start,
                        End = DateTime.Now.AddSeconds(-5)//this is to 'hide' the 5 stop delay that allows the sprinklers to drain
                    });
                }
            }
            //manual or weather skip
            else if (context.ActiveWaterEvent != null
                     && context?.ZoneToRun != null
                     && context?.ZoneToRun?.ZoneId == context?.ActiveWaterEvent?.ZoneName
                     && (context?.IsManualRainDelay == true
                     || context?.IsWeatherApiRainDelay == true))
            {
                var h = new WaterEventHistoryCreateModel();
                string msg = $"{context.WeatherData?.WeatherApiResult?.Payload?.weather.FirstOrDefault()?.description}";
                if (context.IsManualRainDelay) msg += " ManualDelay";
                if (context.IsWeatherApiRainDelay) msg += "+WeatherDelay";
                h.Message = msg;
                h.Start = DateTime.Now;
                h.ZoneName = context.ZoneToRun.ZoneId;
                await _sprinklerApi.WaterEventHistoryCreate(h);

                //otherwise it adds a history every 30 sec :/
                await Task.Delay(context.ActiveWaterEvent.End - context.ActiveWaterEvent.Start);
            }

            return context;
        }
    }
}
