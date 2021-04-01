namespace SprinklerAgent.App
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Configuration;
    using Logging;
    using LogicLadder;
    using Microsoft.ApplicationInsights.DataContracts;
    using Models;
    using Newtonsoft.Json;

    public class App
    {
        private readonly Ladder<SprinklerAgentContext> _appLogic;
        private readonly IAppLogger _logger;
        private readonly IConfig _config;
        private readonly Func<bool> runUntil;

        public App(
            Ladder<SprinklerAgentContext> appLogic,
            IAppLogger logger,
            IConfig config,
            Func<bool> runUntil = null)
        {
            _appLogic = appLogic;
            _logger = logger;
            _config = config;
            this.runUntil = runUntil;
        }

        public void Run()
        {
            _logger.Log(message: "App.Run", level: SeverityLevel.Information);
            while (runUntil?.Invoke() ?? true)
            {
                try
                {
                    var ladderResult = _appLogic.Run(new SprinklerAgentContext(), new CancellationToken()).Result;
                    _logger.Log(
                        message: "Run_Finished",
                        level: SeverityLevel.Information,
                        properties: new Dictionary<string, string>()
                        {
                            {LogLabels.LadderResult, JsonConvert.SerializeObject(ladderResult)}
                        });
                }
                catch (Exception ex)
                {
                    _logger.Log(ex);
                }

                Thread.Sleep(TimeSpan.FromSeconds(_config.RunFrequencySeconds()));
            }
        }
    }
}
