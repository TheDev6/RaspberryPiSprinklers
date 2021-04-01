namespace SprinklerApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AppLogic;
    using Data.Tables;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    [Route("api/[controller]")]
    public class WaterEventHistoryController : ControllerBase
    {
        private readonly WaterEventHistoryLogic _eventHistoryLogic;

        public WaterEventHistoryController(WaterEventHistoryLogic eventHistoryLogic)
        {
            _eventHistoryLogic = eventHistoryLogic;
        }

        [HttpGet]
        [Route("GetByDateRange")]
        public async Task<StandardResponse<List<WaterEventHistory>>> GetByDateRange([FromQuery] DateTime start, DateTime end)
        {
            var result = await _eventHistoryLogic.GetByDateRange(start: start, end: end);
            Response.StatusCode = result.StatusCode;
            return result;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<StandardResponse<WaterEventHistory>> Create([FromBody]WaterEventHistoryCreateModel model)
        {
            var result = await _eventHistoryLogic.Create(model);
            Response.StatusCode = result.StatusCode;
            return result;
        }

        [HttpPut]
        [Route("Update")]
        public async Task<StandardResponse<WaterEventHistory>> Update([FromBody]WaterEventHistoryUpdateModel model)
        {
            var result = await _eventHistoryLogic.Update(model);
            Response.StatusCode = result.StatusCode;
            return result;
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<StandardResponse<int>> Delete([FromQuery]Guid waterEventHistoryUid)
        {
            var result = await _eventHistoryLogic.Delete(waterEventHistoryUid);
            Response.StatusCode = result.StatusCode;
            return result;
        }
    }
}
