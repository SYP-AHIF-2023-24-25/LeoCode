using System.Text.Json.Serialization;
using DotNetTestService.Core;
using DotNetTestService.Core.CheckRuns;
using DotNetTestService.Core.FileHandling;
using DotNetTestService.Core.Stats;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DotNetTestService
{
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
            services.Configure<AppSettings>(Configuration.GetSection(AppSettings.KEY));
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    var enumConverter = new JsonStringEnumConverter();
                    options.JsonSerializerOptions.Converters.Add(enumConverter);
                    options.JsonSerializerOptions.IgnoreNullValues = false;
                });
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new() {Title = "DotNetTestService", Version = "v1"}); });

            SetupDependencies(services);
        }

        private static void SetupDependencies(IServiceCollection services)
        {
            services.AddSingleton<ICodeCheckService, CodeCheckService>();
            services.AddSingleton<ICheckRunService, CheckRunService>();
            services.AddSingleton<IStatsService, StatsService>();

            services.AddTransient<IProjectDefProvider, ProjectDefProvider>();
            services.AddTransient<IRunFileHandler, RunFileHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotNetTestService v1"));

                app.UseCors(x => x
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true) // allow any origin
                    .AllowCredentials()); // allow credentials
            }

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}