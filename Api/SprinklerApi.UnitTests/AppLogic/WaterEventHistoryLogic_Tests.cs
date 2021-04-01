//namespace SprinklerApi.UnitTests.AppLogic
//{
//    using System;
//    using System.Threading.Tasks;
//    using Configuration;
//    using Data;
//    using Models;
//    using NSubstitute;
//    using SprinklerApi.AppLogic;
//    using SprinklerApi.AppLogic.Validators;
//    using Xunit;

//    public class WaterEventHistoryLogic_Tests
//    {
//        [Fact]
//        public async Task Update()
//        {
//            var config = Substitute.For<IConfig>();
//            config.SqlLiteConnectionString()
//                .ReturnsForAnyArgs("Data Source=sprinkler.db;");
//            var dataClient = new SprinklerDataClient(context: SprinklerDbContext.Build(config));
//            var createValidator = new WaterEventHistory_CreateValidator();
//            var updateValidator = new WaterEventHistory_UpdateValidator(dataClient);

//            var sut = new WaterEventHistoryLogic(
//                dataClient: dataClient,
//                createValidator: createValidator,
//                updateValidator: updateValidator);

//            var createResult = await sut.Create(new WaterEventHistoryCreateModel
//            {
//                Message = "Message",
//                Start = DateTime.Now,
//                End = DateTime.Now.AddMinutes(5),
//                ZoneName = "Trees"
//            });

//            try
//            {
//                var model = new WaterEventHistoryUpdateModel
//                {
//                    Message = createResult.Payload.Message,
//                    ZoneName = createResult.Payload.ZoneName,
//                    Start = createResult.Payload.Start,
//                    End = DateTime.Now,
//                    WaterEventHistoryUid = createResult.Payload.WaterEventHistoryUid
//                };
//                var updateResult = await sut.Update(model);
//            }
//            catch (Exception ex)
//            {
//                var x = ex;
//            }

//        }
//    }
//}
