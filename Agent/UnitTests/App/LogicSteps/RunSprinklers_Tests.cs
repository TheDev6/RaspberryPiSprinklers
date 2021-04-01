//namespace UnitTests.App.LogicSteps
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Threading.Tasks;
//    using FluentAssertions;
//    using Microsoft.ApplicationInsights.DataContracts;
//    using SprinklerAgent.App.LogicSteps;
//    using Xunit;
//    using NSubstitute;
//    using SprinklerAgent.App;
//    using SprinklerAgent.App.Models;
//    using SprinklerAgent.Logging;
//    using Xunit.Sdk;

//    public class RunSprinklers_Tests
//    {
//        [Fact]
//        public async Task Run_ActiveWaterEvent()
//        {
//            var logger = Substitute.For<IAppLogger>();
//            var sprinklerManager = Substitute.For<ISprinklerManager>();
//            var runZoneRan = false;
//            sprinklerManager
//                .When(s => s.RunZone(
//                        zone: Arg.Any<SprinklerZone>(),
//                        runSpan: Arg.Any<TimeSpan>()))
//                .Do(ctx => runZoneRan = true);
//            sprinklerManager.When(s => s.RunZone(
//                    zone: Arg.Any<SprinklerZone>(),
//                    runSpan: Arg.Any<TimeSpan>()))
//                .Do(ctx => runZoneRan = true);
//            var logs = new List<dynamic>();
//            logger.When(x => x.Log(
//                message: Arg.Any<string>(),
//                level: Arg.Any<SeverityLevel>(),
//                properties: Arg.Any<Dictionary<string, string>>()))
//                .Do(ctx =>
//                {
//                    dynamic obj = new
//                    {
//                        Message = ctx[0],
//                        Level = ctx[1],
//                        Properties = ctx[2]
//                    };
//                    logs.Add(obj);
//                });
//            var ctx = new SprinklerAgentContext();
//            ctx.ZoneToRun = SprinklerZones.NorthGrass;
//            ctx.ActiveWaterEvent = new WeeklyWaterEventModel
//            {
//                ZoneName = ZoneLabels.NorthGrass,
//                DayOfWeek = DateTime.Now.DayOfWeek,
//                Start = DateTime.Now.TimeOfDay,
//                End = DateTime.Now.TimeOfDay.Add(TimeSpan.FromSeconds(2))
//            };
//            var sut = new RunSprinklers(sprinklerManager: sprinklerManager);

//            var result = await sut.RunStep(context: ctx);

//            runZoneRan.Should().BeTrue();
//        }
//    }
//}