namespace Unit_Tests.ValidatorTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Jobz.WebUi.Models;
    using Jobz.WebUi.Validation.Validators;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RunScheduleUpdateValidator_Tests
    {
        [TestMethod]
        public async Task Validate_OverlapDate()
        {
            var sut = new RunScheduleUpdateValidator();
            var model = new ScheduleUpdateModel()
            {
                WaterEvents = new List<WaterEventCreateModel>
                {
                    new WaterEventCreateModel
                    {
                        ZoneId = "Trees",
                        DayOfWeek = "Monday",
                        StartHour = 1,
                        StartMinute = 0,
                        StartSecond = 13,
                        DurationMinutes = 30,
                        DurationSeconds = 30,
                        LiveId = Guid.NewGuid().ToString()
                    },
                    new WaterEventCreateModel
                    {
                        ZoneId = "Trees",
                        DayOfWeek = "Monday",
                        StartHour = 1,
                        StartMinute = 0,
                        StartSecond = 13,
                        DurationMinutes = 30,
                        DurationSeconds = 30,
                        LiveId = Guid.NewGuid().ToString()
                    }
                }
            };

            var result = await sut.ValidateAsync(model);
            result.IsValid.Should().BeFalse();
            result.Errors.Count.Should().Be(1);

        }

        [TestMethod]
        public async Task Validate_Valid()
        {
            var sut = new RunScheduleUpdateValidator();
            var model = new ScheduleUpdateModel()
            {
                WaterEvents = new List<WaterEventCreateModel>
                {
                    new WaterEventCreateModel
                    {
                        ZoneId = "Trees",
                        DayOfWeek = "Monday",
                        StartHour = 1,
                        StartMinute = 0,
                        StartSecond = 13,
                        DurationMinutes = 30,
                        DurationSeconds = 30,
                        LiveId = Guid.NewGuid().ToString()
                    },
                    new WaterEventCreateModel
                    {
                        ZoneId = "Trees",
                        DayOfWeek = "Tuesday",
                        StartHour = 1,
                        StartMinute = 0,
                        StartSecond = 13,
                        DurationMinutes = 30,
                        DurationSeconds = 30,
                        LiveId = Guid.NewGuid().ToString()
                    }
                }
            };

            var result = await sut.ValidateAsync(model);
            result.Errors.Count.Should().Be(0);
            result.IsValid.Should().BeTrue();
        }

        [TestMethod]
        public async Task Validate_InvalidZone()
        {
            var sut = new RunScheduleUpdateValidator();
            var model = new ScheduleUpdateModel()
            {
                WaterEvents = new List<WaterEventCreateModel>
                {
                    new WaterEventCreateModel
                    {
                        ZoneId = "Treesz",
                        DayOfWeek = "Monday",
                        StartHour = 1,
                        StartMinute = 0,
                        StartSecond = 13,
                        DurationMinutes = 30,
                        DurationSeconds = 30,
                        LiveId = Guid.NewGuid().ToString()
                    },
                    new WaterEventCreateModel
                    {
                        ZoneId = "Tree",
                        DayOfWeek = "Monday",
                        StartHour = 2,
                        StartMinute = 0,
                        StartSecond = 13,
                        DurationMinutes = 30,
                        DurationSeconds = 30,
                        LiveId = Guid.NewGuid().ToString()
                    }
                }
            };

            var result = await sut.ValidateAsync(model);
            result.IsValid.Should().BeFalse();
            result.Errors.Count.Should().Be(2);

        }

        [TestMethod]
        public async Task Validate_InvalidDay()
        {
            var sut = new RunScheduleUpdateValidator();
            var model = new ScheduleUpdateModel()
            {
                WaterEvents = new List<WaterEventCreateModel>
                {
                    new WaterEventCreateModel
                    {
                        ZoneId = "Trees",
                        DayOfWeek = "Mondayd",
                        StartHour = 1,
                        StartMinute = 0,
                        StartSecond = 13,
                        DurationMinutes = 30,
                        DurationSeconds = 30,
                        LiveId = Guid.NewGuid().ToString()
                    },
                    new WaterEventCreateModel
                    {
                        ZoneId = "Trees",
                        DayOfWeek = "barkday",
                        StartHour = 2,
                        StartMinute = 0,
                        StartSecond = 13,
                        DurationMinutes = 30,
                        DurationSeconds = 30,
                        LiveId = Guid.NewGuid().ToString()
                    }
                }
            };

            var result = await sut.ValidateAsync(model);
            result.IsValid.Should().BeFalse();
            result.Errors.Count.Should().Be(2);

        }
    }


}
