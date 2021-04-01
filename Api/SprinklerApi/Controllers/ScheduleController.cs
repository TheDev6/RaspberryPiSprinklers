namespace SprinklerApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using AppLogic;
    using Data.Tables;
    using Models;

    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly WeeklyWaterEventLogic _waterEventLogic;

        public ScheduleController(WeeklyWaterEventLogic waterEventLogic)
        {
            _waterEventLogic = waterEventLogic;
        }

        [HttpGet]
        [Route("GetList")]
        public async Task<StandardResponse<List<WeeklyWaterEvent>>> GetList()
        {
            var result = await _waterEventLogic.GetAll();
            return result;
        }

        [HttpGet]
        [Route("GetSchedule")]
        public async Task<StandardResponse<ScheduleModel>> GetSchedule()
        {
            var result = await _waterEventLogic.GetSchedule();
            return result;
        }

        [HttpGet]
        [Route("GetItem")]
        public async Task<StandardResponse<WeeklyWaterEvent>> GetItem([FromQuery]Guid weeklyWaterEventGuid)
        {
            var result = await _waterEventLogic.Get(weeklyWaterEventGuid);
            return result;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<StandardResponse<WeeklyWaterEvent>> Create([FromBody] WeeklyWaterEventCreateModel model)
        {
            var result = await _waterEventLogic.Create(model);
            Response.StatusCode = result.StatusCode;
            return result;
        }

        [HttpPut]
        [Route("Update")]
        public async Task<StandardResponse<WeeklyWaterEvent>> Update([FromBody] WeeklyWaterEventUpdateModel model)
        {
            var result = await _waterEventLogic.Update(model);
            return result;
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<StandardResponse<Guid>> DeleteItem(Guid weeklyWaterEventUid)
        {
            var result = await _waterEventLogic.Delete(weeklyWaterEventUid);
            Response.StatusCode = result.StatusCode;
            return result;
        }
    }
}
