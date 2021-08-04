using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newbe.Claptrap.Bootstrapper;
using OrderClaptrap.Actors;
using OrderClaptrap.Actors.AuctionItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newbe.Claptrap.Dapr.Hosting.Tests
{
    public class Startup
    {
        private readonly AutofacClaptrapBootstrapper _claptrapBootstrapper;
        private readonly IClaptrapDesignStore _claptrapDesignStore;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var loggerFactory = new ServiceCollection()
                .AddLogging(logging => logging.AddConsole())
                .BuildServiceProvider()

                .GetRequiredService<ILoggerFactory>();

            var bootstrapperBuilder = new AutofacClaptrapBootstrapperBuilder(loggerFactory);
            _claptrapBootstrapper = (AutofacClaptrapBootstrapper)bootstrapperBuilder
                .ScanClaptrapModule()
                .AddConfiguration(configuration)
                .ScanClaptrapDesigns(new[] { typeof(AuctionItemActor).Assembly })
                //.UseDaprPubsub(pubsub => pubsub.AsEventCenter())
                .Build();
            _claptrapDesignStore = _claptrapBootstrapper.DumpDesignStore();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ServiceInitial(services, Configuration);
            services.AddApplication<OrderClaptrapBackendModule>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac here. Don't
            // call builder.Populate(), that happens in AutofacServiceProviderFactory
            // for you.
            builder.RegisterModule<ActorsModule>();

            _claptrapBootstrapper.Boot(builder);
        }

        private void ServiceInitial(IServiceCollection services, IConfiguration configuration)
        {
            services.AddClaptrapServerOptions();
            //services.AddOpenTelemetryTracing(
            //    builder => builder
            //        .AddSource(ClaptrapActivitySource.Instance.Name)
            //        .SetSampler(new ParentBasedSampler(new AlwaysOnSampler()))
            //        .AddAspNetCoreInstrumentation()
            //        .AddGrpcClientInstrumentation()
            //        .AddHttpClientInstrumentation()
            //        .AddZipkinExporter(options =>
            //        {
            //            var zipkinBaseUri = configuration.GetServiceUri("zipkin", "http");
            //            options.Endpoint = new Uri(zipkinBaseUri!, "api/v2/spans");
            //        })
            //);

            services.AddClaptrapServerOptions();
            services.AddActors(options => { options.AddClaptrapDesign(_claptrapDesignStore); });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderClaptrap.BackendServer v1"));
            }

            app.UseRouting();

            //app.UseCloudEvents();

            //app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapActorsHandlers();
            //    endpoints.MapSubscribeHandler();
            //    endpoints.MapControllers();
            //});
        }
    }
}