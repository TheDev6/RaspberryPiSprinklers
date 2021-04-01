namespace Jobz.WebUi.Security.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;
    using Models;
    using Newtonsoft.Json;
    using RootContracts.BehaviorContracts.Configuration;
    using RootContracts.BehaviorContracts.Utilities;
    using Validation;

    public class SecureActionAttribute : AuthorizeAttribute
    {
        public string AreaName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public bool IsJson { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var result = false;

            if (httpContext.User.Identity.IsAuthenticated)
            {
                var logger = IocConfig.Container.GetInstance<ILogger>();
                var config = IocConfig.Container.GetInstance<IConfig>();

                //corresponds to ClaimTypes.NameIdentifier, user.AppUserGuid.ToString() in Authenticator
                var userID = httpContext.GetOwinContext()?.Request?.User?.Identity?.GetUserId() ?? "";
                var name = httpContext.GetOwinContext()?.Request?.User?.Identity?.GetUserName() ?? "";

                try
                {
                    if (config.SecureControllerActions())
                    {
                        //var db = IocConfig.Container.GetInstance<IJobzContext>();
                        //var cache = IocConfig.Container.GetInstance<ICacheUtil>();
                        //var appUserGuid = new Guid(userID);

                        //result = cache.GetCacheThenSource(
                        //    key: $"{appUserGuid}_{this.AreaName}_{this.ControllerName}_{this.ActionName}",
                        //    source: () => db.AppUserHasAction(
                        //            appUserGuid: appUserGuid,
                        //            areaName: AreaName,
                        //            controllerName: ControllerName,
                        //            actionName: ActionName)
                        //        .Single().HasAction,
                        //    cacheExpireMinutes: 1
                        //);

                        //if (!result)//log access denied events
                        //{
                        //    logger.TrackEvent(
                        //        name: CustomEventNames.AccessDenied,
                        //        properties: new Dictionary<string, string>
                        //        {
                        //            {"AppUserGuid", userID},
                        //            {"Name", name},
                        //            {"AreaName", this.AreaName},
                        //            {"ControllerName", this.ControllerName},
                        //            {"ActionName", this.ActionName},
                        //            {"IsJson", this.IsJson.ToString()}
                        //        });
                        //}
                    }
                    else
                    {
                        result = true;
                    }
                }
                catch (Exception ex)
                {
                    logger?.TrackException(
                        exception: ex,
                        properties: new Dictionary<string, string>
                        {
                            {"AppUserGuid", userID},
                            {"Name", name},
                            {"AreaName", this.AreaName},
                            {"ControllerName", this.ControllerName},
                            {"ActionName", this.ActionName},
                            {"IsJson", this.IsJson.ToString()}
                        });
                    result = false;
                }
            }

            if (!result && this.IsJson)
            {
                httpContext.Response.SuppressFormsAuthenticationRedirect = true;
                var ajaxResult = new AjaxResponse<object>();
                ajaxResult.ValidationResult.ValidationFailures.Add(new ValidationFailure
                {
                    Message = "Access Denied. It does not appear that you have permission to call this method."
                });
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.Write(JsonConvert.SerializeObject(ajaxResult));
                httpContext.Response.End();
            }
            return result;
        }
    }
}