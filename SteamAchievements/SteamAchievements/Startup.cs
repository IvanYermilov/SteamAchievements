using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SteamAchievements.Application.ActionFilters;
using SteamAchievements.Extensions;
using System.Reflection;

namespace SteamAchievements
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
            var assembly = Assembly.Load("SteamAchievements.Application");
            services.ConfigureCors();
            services.ConfigureIISIntegration();
            services.ConfigureRepositoryManager();
            //services.Configure<ApiBehaviorOptions>(options =>
            //{
            //    options.SuppressModelStateInvalidFilter = true;
            //});
            services.ConfigureSqlContext(Configuration);
            services.AddAutoMapper(assembly);
            services.AddControllers(config =>
            {
                config.RespectBrowserAcceptHeader = true;
                //    config.ReturnHttpNotAcceptable = true;
                //    config.CacheProfiles.Add("120SecondsDuration", new CacheProfile
                //    {
                //        Duration = 120
                //    });
            }).AddNewtonsoftJson()
            .AddXmlDataContractSerializerFormatters();
            //    .AddCustomCSVFormatter();
            //services.AddCustomMediaTypes();
            services.AddHttpContextAccessor();
            services.ConfigureServices();
            services.ConfigureActiveFilters();
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
            services.ConfigureJWT(Configuration);
            //services.AddScoped<IAuthenticationManager, AuthenticationManager>();
            //services.ConfigureSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            
            app.ConfigureExceptionHandler(logger);

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
