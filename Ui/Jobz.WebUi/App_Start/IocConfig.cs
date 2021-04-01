namespace Jobz.WebUi
{
    using System.Reflection;
    using System.Web.Mvc;
    using AzureBlob;
    using Logic;
    using Microsoft.ApplicationInsights;
    using RootContracts.BehaviorContracts.Configuration;
    using RootContracts.BehaviorContracts.Utilities;
    using RootContracts.Security;
    using Security;
    using SimpleInjector;
    using SimpleInjector.Integration.Web.Mvc;
    using Sprinkler_Api;
    using Utilities;
    using Validation.Validators;

    public static class IocConfig
    {
        public static Container Container;
        public static void RegisterComponents()
        {
            var container = new Container();

            //container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            //container.Options.DefaultLifestyle = Lifestyle.Scoped;

            #region TypeRegistrations
            container.Register<IConfig, Config>(Lifestyle.Singleton);
            container.Register<ICacheUtil, CacheUtil>(Lifestyle.Singleton);
            container.Register<TelemetryClient>(() =>
            {
                //the key also exists in the applicationinsights config.
                var tc = new TelemetryClient();
                tc.InstrumentationKey = container.GetInstance<IConfig>().AiInstrumentKey();
                return tc;
            }, Lifestyle.Singleton);
            container.Register<ILogger, Logger>(Lifestyle.Singleton);
            container.Register<IHashUtility, HashUtility>(Lifestyle.Singleton);
            container.Register<IAuthenticator, Authenticator>(Lifestyle.Singleton);
            container.Register<AzureBlobClient>(Lifestyle.Singleton);
            container.Register<RunScheduleUpdateValidator>(Lifestyle.Singleton);
            container.Register<RainDelayUpdateModelValidator>(Lifestyle.Singleton);
            container.Register<LogicHandler>(Lifestyle.Singleton);
            container.Register<ISprinklerApiClient, SprinklerApiClient>(Lifestyle.Singleton);

            #endregion

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.Verify();
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
            Container = container;
        }
    }
}