using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using SteamAchievements.ActionFilters;
using SteamAchievements.Extensions;
using System.IO;

namespace SteamAchievements
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();
            services.ConfigureIISIntegration();
            services.ConfigureLoggerService();
            services.ConfigureRepositoryManager();
            //services.Configure<ApiBehaviorOptions>(options =>
            //{
            //    options.SuppressModelStateInvalidFilter = true;
            //});
            services.ConfigureSqlContext(Configuration);
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
            //services.AddControllers(config =>
            //{
            //    config.RespectBrowserAcceptHeader = true;
            //    config.ReturnHttpNotAcceptable = true;
            //    config.CacheProfiles.Add("120SecondsDuration", new CacheProfile
            //    {
            //        Duration = 120
            //    });
            //}).AddNewtonsoftJson()
            //    .AddXmlDataContractSerializerFormatters()
            //    .AddCustomCSVFormatter();
            //services.AddCustomMediaTypes();
            services.AddScoped<ValidationFilterAttribute>();
            services.AddScoped<ValidateDeveloperExistsAttribute>();
            services.AddScoped<ValidateGameForDeveloperExistsAttribute>();
            services.AddScoped<ValidateDeveloperForGameExistsAttribute>();
            //services.AddScoped<ValidateMediaTypeAttribute>();
            //services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();
            //services.AddScoped<EmployeeLinks>();
            //services.ConfigureVersioning();
            //services.ConfigureResponseCaching();
            //services.ConfigureHttpCacheHeaders();
            //services.AddMemoryCache();
            //services.AddInMemoryRateLimiting();
            //services.ConfigureRateLimitingOptions();
            //services.AddHttpContextAccessor();
            services.AddAuthentication();
            services.ConfigureIdentity();
            //services.ConfigureJWT(Configuration);
            //services.AddScoped<IAuthenticationManager, AuthenticationManager>();
            //services.ConfigureSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //app.ConfigureExceptionHandler(logger);

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            //app.UseResponseCaching();

            //app.UseHttpCacheHeaders();

            //app.UseIpRateLimiting();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //app.UseSwagger();
            //app.UseSwaggerUI(s =>
            //{
            //    s.SwaggerEndpoint("/swagger/v1/swagger.json", "Steam Achievements API v1");
            //    s.SwaggerEndpoint("/swagger/v2/swagger.json", "Steam Achievements API v2");
            //});
        }
    }
}
