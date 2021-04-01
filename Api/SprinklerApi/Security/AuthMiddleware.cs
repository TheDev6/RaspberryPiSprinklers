namespace SprinklerApi.Security
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Logging;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.AspNetCore.Http;
    using Models;
    using Newtonsoft.Json;

    public class AuthMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IAppLogger _logger;
        private readonly Authenticator _authenticator;

        public AuthMiddleware(
            RequestDelegate next,
            IAppLogger logger,
            Authenticator authenticator)
        {
            this.next = next;
            _logger = logger;
            _authenticator = authenticator;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                //allow swagger stuff
                bool isAllow = context.Request.Path.ToString().StartsWith("/swagger")
                    || context.Request.Path.ToString().StartsWith("/favicon.ico");

                string authHeader = context.Request.Headers["Authorization"];

                string username = null;
                string password = null;

                //Support for basic auth
                if (!string.IsNullOrWhiteSpace(authHeader)
                    && authHeader.StartsWith("Basic ")
                    && !isAllow)
                {
                    var encodedUsernamePassword =
                        authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();
                    var decodedUsernamePassword =
                        Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));
                    var splitUsernamePass = decodedUsernamePassword.Split(':', 2);
                    username = splitUsernamePass[0];
                    password = splitUsernamePass[1];
                    isAllow = await _authenticator.IsValidUser(username: username, password: password);
                }

                //Support for ApiToken Bearer
                if (!string.IsNullOrWhiteSpace(authHeader)
                    && authHeader.StartsWith("Bearer ")
                    && !isAllow)
                {
                    var usernamePass = authHeader.Replace("Bearer ", "");
                    var splitUsernamePass = usernamePass.Split(':', 2);
                    username = splitUsernamePass[0];
                    password = splitUsernamePass[1];
                    isAllow = await _authenticator.IsValidUser(username: username, password: password);
                }

                if (isAllow)
                {
                    await next.Invoke(context);
                    return;
                }
                else
                {
                    _logger.Log(message: $"Failed Login. Username:{username} Password:{password}", level: SeverityLevel.Warning);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
            }

            var deniedResponse = new StandardResponse<string>()
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
                ValidationMessages = new List<string>()
                {
                    "Invalid Login. Either the credentials were incorrect or not provided. Add header 'Authorization' with value 'Bearer username:password'"
                }
            };
            var deniedJsonBody = JsonConvert.SerializeObject(deniedResponse);
            context.Response.StatusCode = deniedResponse.StatusCode;
            await context.Response.WriteAsync(text: deniedJsonBody, encoding: Encoding.UTF8);
        }
    }
}