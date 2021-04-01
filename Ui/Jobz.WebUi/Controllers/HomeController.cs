namespace Jobz.WebUi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Logic;
    using Models;
    using Sprinkler_Api.Models;
    using Utilities;

    public class HomeController : Controller
    {
        private readonly LogicHandler _handler;

        public HomeController(LogicHandler handler)
        {
            _handler = handler;
        }
        public async Task<ActionResult> Index()
        {
            return View();
        }

        [Authorize]
        public async Task<ActionResult> Schedule()
        {
            var schedule = await _handler.GetSchedule();
            return View(schedule);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> DeleteWaterEvent(Guid weeklyWaterEventUid)
        {
            var deleteResult = await _handler.DeleteWaterEvent(weeklyWaterEventUid: weeklyWaterEventUid);
            return new JsonNetResult(deleteResult);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateWaterEvent(WeeklyWaterEventCreateModel model)
        {
            var createResponse = await _handler.CreateWaterEvent(model);
            return new JsonNetResult(createResponse);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> UpdateWaterEvent(WeeklyWaterEventUpdateModel model)
        {
            var createResponse = await _handler.UpdateWaterEvent(model);
            return new JsonNetResult(createResponse);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> RainDelayPost(RainDelayCreateModel model)
        {
            var result = await _handler.RainDelayCreate(model);
            return new JsonNetResult(result);
        }

        [Authorize]
        public async Task<ActionResult> RainDelay()
        {
            var model = await _handler.GetActiveRainDelay();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> RainDelayDelete(Guid rainDelayUid)
        {
            await _handler.DeleteRainDelay(rainDelayUid);
            return new JsonNetResult(new AjaxResponse<int>());
        }

        [Authorize]
        public async Task<ActionResult> History()
        {
            var histories = await _handler.GetWaterEventHistories(
                start: DateTime.Now.AddDays(-14),
                end: DateTime.Now);
            return View(histories.Payload?.OrderByDescending(x => x.Start)?.ToList() ?? new List<WaterEventHistory>());
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> HistoryByDate(DateTime start, DateTime end)
        {
            var histories = await _handler.GetWaterEventHistories(
                start: start,
                end: end);
            return new JsonNetResult(histories);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> HistoryDelete(Guid waterEventHistoryUid)
        {
            var result = await _handler.DeleteWaterEventHistory(waterEventHistoryUid);
            return new JsonNetResult(result);
        }
    }
}