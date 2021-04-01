namespace Jobz.WebUi.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Models;
    using Sprinkler_Api;
    using Sprinkler_Api.Models;
    using Validation;

    public class  LogicHandler
    {
        private readonly ISprinklerApiClient _sprinklerApi;

        public LogicHandler(ISprinklerApiClient sprinklerApi)
        {
            _sprinklerApi = sprinklerApi;
        }

        public async Task<ScheduleViewModel> GetSchedule()
        {
            var result = new ScheduleViewModel();
            var response = await _sprinklerApi.GetWaterEvents();
            result.WaterEventItems = response?.Payload?.Select(w => new WeeklyWaterEventModel
            {
                WeeklyWaterEventUid = w.WeeklyWaterEventUid,
                DayOfWeek = w.DayOfWeek.ToString(),
                StartSpan = w.Start,
                EndSpan = w.End,
                ZoneName = w.ZoneName
            })?.ToList();
            return result;
        }

        public async Task<AjaxResponse<Guid>> DeleteWaterEvent(Guid weeklyWaterEventUid)
        {
            var response = await _sprinklerApi.DeleteWaterEvent(weeklyWaterEventUid: weeklyWaterEventUid);
            return new AjaxResponse<Guid>()
            {
                Payload = response.Payload,
                ValidationResult = new ValidationResult()
                {
                    ValidationFailures = response.ValidationMessages
                        .Select(x => new ValidationFailure() { Message = x })
                        .ToList()
                }
            };
        }

        public async Task<AjaxResponse<WeeklyWaterEvent>> CreateWaterEvent(WeeklyWaterEventCreateModel model)
        {
            var response = await _sprinklerApi.CreateWaterEvent(model);
            return new AjaxResponse<WeeklyWaterEvent>()
            {
                Payload = response.Payload,
                ValidationResult = new ValidationResult()
                {
                    ValidationFailures = response.ValidationMessages
                        .Select(x => new ValidationFailure() { Message = x })
                        .ToList()
                }
            };
        }

        public async Task<AjaxResponse<WeeklyWaterEvent>> UpdateWaterEvent(WeeklyWaterEventUpdateModel model)
        {
            var response = await _sprinklerApi.UpdateWaterEvent(model);
            return new AjaxResponse<WeeklyWaterEvent>()
            {
                Payload = response.Payload,
                ValidationResult = new ValidationResult()
                {
                    ValidationFailures = response.ValidationMessages
                        .Select(x => new ValidationFailure() { Message = x })
                        .ToList()
                }
            };
        }

        public async Task<RainDelay> GetActiveRainDelay()
        {
            var response = await _sprinklerApi.GetActiveRainDelay();
            return response.Payload;
        }

        public async Task DeleteRainDelay(Guid rainDelayUid)
        {
            await _sprinklerApi.DeleteRainDelay(rainDelayUid);
        }

        public async Task<AjaxResponse<RainDelay>> RainDelayCreate(RainDelayCreateModel model)
        {
            var response = await _sprinklerApi.CreateRainDelay(model);
            return new AjaxResponse<RainDelay>()
            {
                Payload = response.Payload,
                ValidationResult = new ValidationResult()
                {
                    ValidationFailures = response.ValidationMessages
                        .Select(x => new ValidationFailure() { Message = x })
                        .ToList()
                }
            };
        }

        public async Task<AjaxResponse<List<WaterEventHistory>>> GetWaterEventHistories(DateTime start, DateTime end)
        {
            var response = await _sprinklerApi.WaterEventHistoryByDateRange(start.Date, end.AddDays(1).Date);
            return new AjaxResponse<List<WaterEventHistory>>()
            {
                Payload = response.Payload,
                ValidationResult = new ValidationResult()
                {
                    ValidationFailures = response.ValidationMessages
                        .Select(x => new ValidationFailure() { Message = x })
                        .ToList()
                }
            };
        }

        public async Task<AjaxResponse<int>> DeleteWaterEventHistory(Guid waterEventHistoryUid)
        {
            var response = await _sprinklerApi.WaterEventHistoryDelete(waterEventHistoryUid);
            return new AjaxResponse<int>()
            {
                Payload = response.Payload,
                ValidationResult = new ValidationResult()
                {
                    ValidationFailures = response.ValidationMessages
                        .Select(x => new ValidationFailure() { Message = x })
                        .ToList()
                }
            };
        }
    }
}
