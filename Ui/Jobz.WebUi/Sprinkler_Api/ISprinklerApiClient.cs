namespace Jobz.WebUi.Sprinkler_Api
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface ISprinklerApiClient
    {
        Task<StandardResponse<List<WeeklyWaterEvent>>> GetWaterEvents();
        Task<StandardResponse<ScheduleModel>> GetSchedule();
        Task<StandardResponse<WeeklyWaterEvent>> GetWaterEvent(Guid weeklyWaterEventUid);
        Task<StandardResponse<WeeklyWaterEvent>> CreateWaterEvent(WeeklyWaterEventCreateModel model);
        Task<StandardResponse<WeeklyWaterEvent>> UpdateWaterEvent(WeeklyWaterEventUpdateModel model);
        Task<StandardResponse<Guid>> DeleteWaterEvent(Guid weeklyWaterEventUid);

        Task<StandardResponse<RainDelay>> GetActiveRainDelay();
        Task<StandardResponse<RainDelay>> CreateRainDelay(RainDelayCreateModel model);
        Task<StandardResponse<int>> DeleteExpiredRainDelays();
        Task<StandardResponse<int>> DeleteRainDelay(Guid rainDelayUid);

        Task<StandardResponse<List<WaterEventHistory>>> WaterEventHistoryByDateRange(DateTime start, DateTime end);
        Task<StandardResponse<WaterEventHistory>> WaterEventHistoryCreate(WaterEventHistoryCreateModel model);
        Task<StandardResponse<WaterEventHistory>> WaterEventHistoryUpdate(WaterEventHistoryUpdateModel model);
        Task<StandardResponse<int>> WaterEventHistoryDelete(Guid waterEventHistoryUid);
    }
}
