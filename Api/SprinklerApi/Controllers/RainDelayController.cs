namespace SprinklerApi.Controllers
{
    using System;
    using System.Threading.Tasks;
    using AppLogic;
    using Data.Tables;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    [Route("api/[controller]")]
    [ApiController]
    public class RainDelayController : ControllerBase
    {
        private readonly RainDelayLogic _rainDelayLogic;

        public RainDelayController(RainDelayLogic rainDelayLogic)
        {
            _rainDelayLogic = rainDelayLogic;
        }

        [HttpGet]
        [Route("GetActiveRainDelay")]
        public async Task<StandardResponse<RainDelay>> ActiveRainDelay()
        {
            var result = await _rainDelayLogic.GetActiveRainDelay();
            Response.StatusCode = result.StatusCode;
            return result;
        }

        [HttpDelete]
        [Route("DeleteExpiredRainDelays")]
        public async Task<StandardResponse<int>> DeleteExpiredRainDelays()
        {
            var result = await _rainDelayLogic.DeleteExpiredRainDelays();
            Response.StatusCode = result.StatusCode;
            return result;
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<StandardResponse<int>> Delete(Guid rainDelayUid)
        {
            var result = await _rainDelayLogic.Delete(rainDelayUid);
            Response.StatusCode = result.StatusCode;
            return result;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<StandardResponse<RainDelay>> Create(RainDelayCreateModel model)
        {
            var result = await _rainDelayLogic.CreateRainDelay(model);
            Response.StatusCode = result.StatusCode;
            return result;
        }
    }
}