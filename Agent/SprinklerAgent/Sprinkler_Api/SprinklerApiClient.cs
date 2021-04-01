namespace SprinklerAgent.Sprinkler_Api
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Configuration;
    using Flurl.Http;
    using Models;

    public class SprinklerApiClient : ISprinklerApiClient
    {
        private readonly IFlurlClient _http;
        public SprinklerApiClient(IConfig config)
        {
            _http = new FlurlClient(config.SprinklerApiUrl());
            _http.Headers.Add("Authorization", config.SprinklerApiCredentials());
        }

        #region WaterEvents
        public async Task<StandardResponse<List<WeeklyWaterEvent>>> GetWaterEvents()
        {
            var result = await _http.Request("api/Schedule/GetList")
                .AllowAnyHttpStatus()
                .GetJsonAsync<StandardResponse<List<WeeklyWaterEvent>>>();
            return result;
        }

        public async Task<StandardResponse<ScheduleModel>> GetSchedule()
        {
            var result = await _http.Request("api/Schedule/GetSchedule")
                .AllowAnyHttpStatus()
                .GetJsonAsync<StandardResponse<ScheduleModel>>();
            return result;
        }

        public async Task<StandardResponse<WeeklyWaterEvent>> GetWaterEvent(Guid weeklyWaterEventUid)
        {
            var result = await _http.Request($"api/Schedule/GetItem?weeklyWaterEventGuid={weeklyWaterEventUid}")
                .AllowAnyHttpStatus()
                .GetJsonAsync<StandardResponse<WeeklyWaterEvent>>();
            return result;
        }

        public async Task<StandardResponse<WeeklyWaterEvent>> CreateWaterEvent(WeeklyWaterEventCreateModel model)
        {
            var response = await _http.Request($"api/Schedule/Create")
                .AllowAnyHttpStatus()
                .PostJsonAsync(model);
            var result = await response.GetJsonAsync<StandardResponse<WeeklyWaterEvent>>();
            return result;
        }

        public async Task<StandardResponse<WeeklyWaterEvent>> UpdateWaterEvent(WeeklyWaterEventUpdateModel model)
        {
            var response = await _http.Request($"/api/Schedule/Update")
                .AllowAnyHttpStatus()
                .PutJsonAsync(model);
            var result = await response.GetJsonAsync<StandardResponse<WeeklyWaterEvent>>();
            return result;
        }

        public async Task<StandardResponse<Guid>> DeleteWaterEvent(Guid weeklyWaterEventUid)
        {
            var response = await _http.Request($"/api/Schedule/Delete?weeklyWaterEventUid={weeklyWaterEventUid}")
                .AllowAnyHttpStatus()
                .DeleteAsync();
            var result = await response.GetJsonAsync<StandardResponse<Guid>>();
            return result;
        }
        #endregion

        #region RainDelay
        public async Task<StandardResponse<RainDelay>> GetActiveRainDelay()
        {
            var result = await _http.Request("api/RainDelay/GetActiveRainDelay")
                .AllowAnyHttpStatus()
                .GetJsonAsync<StandardResponse<RainDelay>>();
            return result;
        }

        public async Task<StandardResponse<RainDelay>> CreateRainDelay(RainDelayCreateModel model)
        {
            var response = await _http.Request("api/RainDelay/Create")
                .AllowAnyHttpStatus()
                .PostJsonAsync(model);
            var result = await response.GetJsonAsync<StandardResponse<RainDelay>>();
            return result;
        }

        public async Task<StandardResponse<int>> DeleteExpiredRainDelays()
        {
            var response = await _http.Request("api/RainDelay/DeleteExpiredRainDelays")
                .AllowAnyHttpStatus()
                .DeleteAsync();
            var result = await response.GetJsonAsync<StandardResponse<int>>();
            return result;
        }

        public async Task<StandardResponse<int>> DeleteRainDelay(Guid rainDelayUid)
        {
            var response = await _http.Request($"/api/RainDelay/Delete?rainDelayUid={rainDelayUid}")
                .AllowAnyHttpStatus()
                .DeleteAsync();
            var result = await response.GetJsonAsync<StandardResponse<int>>();
            return result;
        }
        #endregion

        #region WaterEventHistory
        public async Task<StandardResponse<List<WaterEventHistory>>> WaterEventHistoryByDateRange(DateTime start, DateTime end)
        {
            var result = await _http.Request($"/api/RainDelay/Delete?start={start:O}&end={end:O}")//better way to handle the dates?
                .AllowAnyHttpStatus()
                .GetJsonAsync<StandardResponse<List<WaterEventHistory>>>();
            return result;
        }
        public async Task<StandardResponse<WaterEventHistory>> WaterEventHistoryCreate(WaterEventHistoryCreateModel model)
        {
            var response = await _http.Request($"/api/WaterEventHistory/Create")
                .AllowAnyHttpStatus()
                .PostJsonAsync(model);
            var result = await response.GetJsonAsync<StandardResponse<WaterEventHistory>>();
            return result;
        }
        public async Task<StandardResponse<WaterEventHistory>> WaterEventHistoryUpdate(WaterEventHistoryUpdateModel model)
        {
            var response = await _http.Request($"/api/WaterEventHistory/Update")
                .AllowAnyHttpStatus()
                .PutJsonAsync(model);
            var result = await response.GetJsonAsync<StandardResponse<WaterEventHistory>>();
            return result;
        }
        #endregion
    }
}
