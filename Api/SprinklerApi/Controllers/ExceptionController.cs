namespace SprinklerApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using Logging;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    
    public class ExceptionController : ControllerBase
    {
        private readonly IAppLogger _logger;

        public ExceptionController(IAppLogger logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [HttpPost]
        [Route("/exception")]
        public StandardResponse<object> OnException()
        {
            var ex = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error
                            ?? new Exception("Unknown exception happened.");
            _logger.Log(ex);
            var status = 500;
            Response.StatusCode = status;
            return new StandardResponse<object>()
            {
                ValidationMessages = new List<string>() { $"An Exception occured: {ex.Message}" },
                StatusCode = status
            };
        }
    }
}
