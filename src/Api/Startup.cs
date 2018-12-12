using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlickrWrapper.Api.App;
using FlickrWrapper.Api.DI;
using FlickrWrapper.Api.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;

namespace FlickrWrapper.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        private IServiceProvider _serviceProvider;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
            .AddControllersAsServices()
            .AddJsonOptions(e =>
            {
                e.SerializerSettings.DateFormatString = "yyyy-MM-dd'T'HH:mm:ss.fffK";
                e.SerializerSettings.Culture = System.Globalization.CultureInfo.InvariantCulture;
                e.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });

            services.AddLogging(e => e.ClearProviders());
            
            services.AddTransient<ICorrelationIdSource, CorrelationIdSource>();
            services.AddTransient<IFlickrWrapperApp, FlickrWrapperApp>();
            services.AddHttpClient<IFlickrWrapperApp, FlickrWrapperApp>();

            var flickConfig = _configuration.GetSection("flickr");
            services.AddSingleton(new FlickrConfiguration(
                apiUrl: flickConfig["apiUrl"],
                key: flickConfig["apiKey"],
                secret: flickConfig["apiKeySecret"]
            ));

            _serviceProvider = services.BuildServiceProvider();

            return _serviceProvider;
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IApplicationLifetime appLifetime)
        {
            NLog.LogManager.LoadConfiguration("nlog.config");
            loggerFactory.AddNLog();

            app.UseMiddleware<LoggingMiddleware>();
            app.UseMvc();

            appLifetime.ApplicationStarted.Register(() =>
            {
                _serviceProvider.GetService<ILogger<Startup>>().LogInformation("Application has started");
            });
            
            appLifetime.ApplicationStopped.Register(() =>
            {
                _serviceProvider.GetService<ILogger<Startup>>().LogInformation("Trying to stop gracefully...");
            });
        }

    }
}
