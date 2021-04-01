namespace Jobz.WebUi.Sprinkler_Api
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Flurl.Http;
    using Models;
    using Newtonsoft.Json;
    using RootContracts.BehaviorContracts.Configuration;

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
            var response = await _http.Request("api/Schedule/GetList")
                .AllowAnyHttpStatus()
                .GetAsync();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<StandardResponse<List<WeeklyWaterEvent>>>(content);
            return result;
        }

        public async Task<StandardResponse<ScheduleModel>> GetSchedule()
        {
            var response = await _http.Request("api/Schedule/GetSchedule")
                .AllowAnyHttpStatus()
                .GetAsync();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<StandardResponse<ScheduleModel>>(content);
            return result;
        }

        public async Task<StandardResponse<WeeklyWaterEvent>> GetWaterEvent(Guid weeklyWaterEventUid)
        {
            var response = await _http.Request($"api/Schedule/GetItem?weeklyWaterEventGuid={weeklyWaterEventUid}")
                .AllowAnyHttpStatus()
                .GetAsync();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<StandardResponse<WeeklyWaterEvent>>(content);
            return result;
        }

        public async Task<StandardResponse<WeeklyWaterEvent>> CreateWaterEvent(WeeklyWaterEventCreateModel model)
        {
            var response = await _http.Request($"api/Schedule/Create")
                .AllowAnyHttpStatus()
                .PostJsonAsync(model);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<StandardResponse<WeeklyWaterEvent>>(content);
            return result;
        }

        public async Task<StandardResponse<WeeklyWaterEvent>> UpdateWaterEvent(WeeklyWaterEventUpdateModel model)
        {
            var response = await _http.Request($"/api/Schedule/Update")
                .AllowAnyHttpStatus()
                .PutJsonAsync(model);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<StandardResponse<WeeklyWaterEvent>>(content);
            return result;
        }

        public async Task<StandardResponse<Guid>> DeleteWaterEvent(Guid weeklyWaterEventUid)
        {
            var response = await _http.Request($"/api/Schedule/Delete?weeklyWaterEventUid={weeklyWaterEventUid}")
                .AllowAnyHttpStatus()
                .DeleteAsync();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<StandardResponse<Guid>>(content);
            return result;
        }
        #endregion

        #region RainDelay
        public async Task<StandardResponse<RainDelay>> GetActiveRainDelay()
        {
            var response = await _http.Request("api/RainDelay/GetActiveRainDelay")
                .AllowAnyHttpStatus()
                .GetAsync();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<StandardResponse<RainDelay>>(content);
            return result;
        }

        public async Task<StandardResponse<RainDelay>> CreateRainDelay(RainDelayCreateModel model)
        {
            var response = await _http.Request("api/RainDelay/Create")
                .AllowAnyHttpStatus()
                .PostJsonAsync(model);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<StandardResponse<RainDelay>>(content);
            return result;
        }

        public async Task<StandardResponse<int>> DeleteExpiredRainDelays()
        {
            var response = await _http.Request("api/RainDelay/DeleteExpiredRainDelays")
                .AllowAnyHttpStatus()
                .DeleteAsync();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<StandardResponse<int>>(content);
            return result;
        }

        public async Task<StandardResponse<int>> DeleteRainDelay(Guid rainDelayUid)
        {
            var response = await _http.Request($"/api/RainDelay/Delete?rainDelayUid={rainDelayUid}")
                .AllowAnyHttpStatus()
                .DeleteAsync();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<StandardResponse<int>>(content);
            return result;
        }
        #endregion

        #region WaterEventHistory
        public async Task<StandardResponse<List<WaterEventHistory>>> WaterEventHistoryByDateRange(DateTime start, DateTime end)
        {
            var urlSeg = $"/api/WaterEventHistory/GetByDateRange?start={start}&end={end}";//better way to handle the dates?
            var response = await _http.Request(urlSeg)
                .AllowAnyHttpStatus()
                .GetAsync();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<StandardResponse<List<WaterEventHistory>>>(content);
            return result;
        }
        public async Task<StandardResponse<WaterEventHistory>> WaterEventHistoryCreate(WaterEventHistoryCreateModel model)
        {
            var response = await _http.Request($"/api/WaterEventHistory/Create")
                .AllowAnyHttpStatus()
                .PostJsonAsync(model);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<StandardResponse<WaterEventHistory>>(content);
            return result;
        }
        public async Task<StandardResponse<WaterEventHistory>> WaterEventHistoryUpdate(WaterEventHistoryUpdateModel model)
        {
            var response = await _http.Request($"/api/WaterEventHistory/Update")
                .AllowAnyHttpStatus()
                .PutJsonAsync(model);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<StandardResponse<WaterEventHistory>>(content);
            return result;
        }
        public async Task<StandardResponse<int>> WaterEventHistoryDelete(Guid waterEventHistoryUid)
        {
            var response = await _http.Request($"/api/WaterEventHistory/Delete?waterEventHistoryUid={waterEventHistoryUid}")
                .AllowAnyHttpStatus()
                .DeleteAsync();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<StandardResponse<int>>(content);
            return result;
        }
        #endregion
    }
}
