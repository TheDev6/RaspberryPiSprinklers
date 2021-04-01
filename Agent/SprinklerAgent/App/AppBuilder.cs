namespace SprinklerAgent.App
{
    using System;
    using System.Collections.Generic;
    using AzureBlob;
    using Configuration;
    using DeadSimpleCache;
    using Logging;
    using LogicLadder;
    using LogicSteps;
    using Microsoft.ApplicationInsights.DataContracts;
    using Models;
    using Sprinkler_Api;
    using Weather;

    public static class AppBuilder
    {
        public static App Build(
            IConfig inConfig = null,
            IAppLogger inAppLogger = null,
            ISprinklerManager inSprinklerManager = null,
            //IAzureBlobClient inBlobClient = null,
            Func<bool> runUntil = null,
            IWeatherApiClient inWeatherApiClient = null,
            ISprinklerApiClient inSprinklerApiClient = null)
        {
            var config = inConfig ?? new Config();
            var logger = inAppLogger ?? new AppLogger(config: config);
            var sprinklerManager = inSprinklerManager ?? new SprinklerManager(logger);
            //var blobClient = inBlobClient ?? new AzureBlobClient(config: config);
            var weatherClient = inWeatherApiClient ?? new WeatherApiClient(config: config);
            var cache = new SimpleCache();//one per app if you want to share cache.
            var sprinklerApiClient = inSprinklerApiClient ?? new SprinklerApiClient(config: config);
            logger.Log(message: "Hello Logger!", level: SeverityLevel.Information);
            var appLogic = new Ladder<SprinklerAgentContext>(
                steps: new List<IStep<SprinklerAgentContext>>
                {
                    new GetSchedule(sprinklerApi:sprinklerApiClient, cache:cache),
                    new GetOfflineSchedule(cache),
                    new GetWeatherData(weatherApi:weatherClient,config:config,cache:cache,logger:logger),
                    new GetActiveWaterEvent(
                        logger: logger,
                        sprinklerManager: sprinklerManager,
                        weatherApiClient:weatherClient,
                        config:config,
                        cache:cache),
                    new GetZoneToRun(logger: logger, sprinklerManager: sprinklerManager),
                    new RunSprinklers(sprinklerManager: sprinklerManager,sprinklerApi:sprinklerApiClient),
                    new DeleteExpiredRainDelay(sprinklerApi:sprinklerApiClient) 
                    
                    //new RelayTest(manager:sprinklerManager)
                },
                onError: ex => logger.Log(ex));

            var result = new App(
                appLogic: appLogic,
                logger: logger,
                config: config,
                runUntil: runUntil);

            return result;
        }
    }
}
