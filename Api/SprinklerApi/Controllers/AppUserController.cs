namespace SprinklerApi.Controllers
{
    using System.Threading.Tasks;
    using AppLogic;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    [Route("api/[controller]")]
    public class AppUserController : ControllerBase
    {
        private readonly AppUserLogic _appUserLogic;

        public AppUserController(AppUserLogic appUserLogic)
        {
            _appUserLogic = appUserLogic;
        }

        [HttpPost]
        [Route("CreateAppUser")]
        public async Task<StandardResponse<AppUserModel>> CreateAppUser(AppUserCreateModel model)
        {
            var result = await _appUserLogic.CreateAppUser(model);
            Response.StatusCode = result.StatusCode;
            return result;
        }
    }
}
