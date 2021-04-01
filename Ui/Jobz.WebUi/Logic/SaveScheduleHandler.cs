namespace Jobz.WebUi.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AzureBlob;
    using Models;
    using Models.SprinklerAgentTypes;
    using Utilities;
    using Validation;
    using Validation.Validators;

    public class SaveScheduleHandler
    {
        private readonly RunScheduleUpdateValidator _validator;
        private readonly AzureBlobClient _azureBlobClient;


        public SaveScheduleHandler(
            RunScheduleUpdateValidator validator,
            AzureBlobClient azureBlobClient)
        {
            _validator = validator;
            _azureBlobClient = azureBlobClient;
        }

        public async Task<JsonNetResult> Handle(ScheduleUpdateModel model)
        {
            var valid = await _validator.ValidateAsync(model);
            if (valid.IsValid)
            {
                var mapped = new List<WeeklyWaterEvent>();
                if (model?.WaterEvents?.Any() == true)
                {
                    foreach (var we in model.WaterEvents)
                    {
                        var start = new TimeSpan(hours: we.StartHour, minutes: we.StartMinute, seconds: we.StartSecond);
                        var end = start.Add(new TimeSpan(hours: 0, minutes: we.DurationMinutes, seconds: we.DurationSeconds));
                        var map = new WeeklyWaterEvent
                        {
                            ZoneId = we.ZoneId,
                            DayOfWeek = ToDayOfWeek(we.DayOfWeek),
                            Start = start,
                            End = end,
                        };
                        mapped.Add(map);
                    }
                }

                var runSchedule = await _azureBlobClient.GetSchedule();//this preserves the rain delay expire date.
                runSchedule.WaterEvents = mapped;
                await _azureBlobClient.SaveSchedule(runSchedule);
            }

            var validationMaps = valid?.Errors
                .Select(e => new ValidationFailure
                {
                    Message = e.ErrorMessage,
                    PropertyName = e.PropertyName
                }).ToList();
            var validationResult = new ValidationResult()
            {
                ValidationFailures = validationMaps
            };

            return new JsonNetResult(new AjaxResponse<object>
            {
                ValidationResult = validationResult
            });
        }

        private DayOfWeek ToDayOfWeek(string input)
        {
            switch (input)
            {
                case "Monday":
                    return DayOfWeek.Monday;
                case "Tuesday":
                    return DayOfWeek.Tuesday;
                case "Wednesday":
                    return DayOfWeek.Wednesday;
                case "Thursday":
                    return DayOfWeek.Thursday;
                case "Friday":
                    return DayOfWeek.Friday;
                case "Saturday":
                    return DayOfWeek.Saturday;
                case "Sunday":
                    return DayOfWeek.Sunday;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}