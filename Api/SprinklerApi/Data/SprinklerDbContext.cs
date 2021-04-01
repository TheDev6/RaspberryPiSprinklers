namespace SprinklerApi.Data
{
    using System;
    using System.Collections.Generic;
    using AppLogic;
    using AppLogic.Validators;
    using Configuration;
    using Logging;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Internal;
    using Security;
    using SprinklerApi.Models;
    using Tables;

    public class SprinklerDbContext : DbContext
    {
        public SprinklerDbContext(DbContextOptions options) : base(options) { }
        public DbSet<WeeklyWaterEvent> WeeklyWaterEvents { get; set; }
        public DbSet<RainDelay> RainDelays { get; set; }
        public DbSet<WaterEventHistory> WaterEventHistories { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            WeeklyWaterEvent.RegisterForSqlLite(builder);
            RainDelay.RegisterForSqlLite(builder);
            WaterEventHistory.RegisterForSqlLite(builder);
            AppUser.RegisterForSqlLite(builder);
        }

        public static SprinklerDbContext Build(IConfig config)
        {
            var logger = new AppLogger(config);
            logger.Log("Build", SeverityLevel.Information);
            var optionBuilder = new DbContextOptionsBuilder<SprinklerDbContext>();
            optionBuilder.UseSqlite(config.SqlLiteConnectionString());
            var context = new SprinklerDbContext(optionBuilder.Options);
            logger.Log("context.Database.EnsureCreated();", SeverityLevel.Information);
            context.Database.EnsureCreated();
            if (context.AppUsers?.Any() == false)//seed and log at least one user, if one doesn't already exist.
            {
                var hashUtil = new HashUtility();
                var dataClient = new SprinklerDataClient(context);
                var createValidator = new AppUser_CreateValidator(dataClient: dataClient);
                var appUserLogic = new AppUserLogic(dataClient: dataClient, hashUtil: hashUtil, createValidator: createValidator);
                var pass = Guid.NewGuid().ToString();
                var userResult = appUserLogic.CreateAppUser(new AppUserCreateModel
                {
                    Username = Guid.NewGuid().ToString(),
                    Password = pass,
                    Claims = new List<string>() { Claims.Basic }
                }).Result;
                logger.Log(
                    message: "New_Seeded_User",
                    level: SeverityLevel.Information,
                    properties: new Dictionary<string, string>()
                    {
                        {"username", userResult.Payload.Username},
                        {"password", pass}
                    });
            }
            return context;
        }
    }
}
