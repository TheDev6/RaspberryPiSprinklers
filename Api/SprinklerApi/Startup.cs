namespace SprinklerApi
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using AppLogic;
    using AppLogic.Validators;
    using AzureBlob;
    using Configuration;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Data;
    using JsonExtenders;
    using Logging;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.OpenApi.Models;
    using Newtonsoft.Json;
    using Security;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var config = new Config();
            var logger = new AppLogger(config);
            try
            {
                logger.Log(message: $"Hello! {this.GetType().Name}", level: SeverityLevel.Verbose);

                services.AddControllers().AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new TimeSpanConverter());
                });


                services.AddSingleton<IConfig, Config>(ctx => config);
                services.AddSingleton<IAppLogger, AppLogger>(ctx => logger);
                services.AddSingleton<ISprinklerDataClient, SprinklerDataClient>();
                services.AddSingleton<HashUtility>();
                services.AddSingleton<Authenticator>();
                services.AddSingleton<AzureBlobClient>();

                services.AddSingleton<SprinklerDbContext>(ctx => SprinklerDbContext.Build(ctx.GetRequiredService<IConfig>()));

                services.AddSingleton<WeeklyWaterEvent_UpdateValidator>();
                services.AddSingleton<WeeklyWaterEvent_CreateValidator>();
                services.AddSingleton<RainDelay_CreateValidator>();
                services.AddSingleton<WaterEventHistory_CreateValidator>();
                services.AddSingleton<WaterEventHistory_UpdateValidator>();
                services.AddSingleton<AppUser_CreateValidator>();

                services.AddSingleton<WeeklyWaterEventLogic>();
                services.AddSingleton<RainDelayLogic>();
                services.AddSingleton<WaterEventHistoryLogic>();
                services.AddSingleton<AppUserLogic>();
                services.AddSingleton<DataBackupRestoreLogic>();


                //https://github.com/domaindrivendev/Swashbuckle.AspNetCore#swashbuckleaspnetcoreswaggergen
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Sprinklers",
                        Version = "1.0"
                    });
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = @"Example:'Bearer yourUserName:yourPassword'",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,

                            },
                            new List<string>()
                        }
                    });
                });
            }
            catch (Exception ex)
            {
                logger.Log(ex);
                File.WriteAllText($"Error_{DateTime.Now.Ticks}.json", JsonConvert.SerializeObject(ex));
                throw;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<AuthMiddleware>();//Security is here!!

            app.UseExceptionHandler("/exception");

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //https://github.com/domaindrivendev/Swashbuckle.AspNetCore
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sprinkler API");
            });
        }
    }
}
