using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newbe.Claptrap.Bootstrapper;
using Newbe.Claptrap.TestSuit.QuickSetupTools;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newbe.Claptrap.Dapr.Hosting.Tests
{
    public class AbpHostTest
    {
        [TestCase("sqlite")]
        [TestCase("mongodb")]
        [TestCase("mysql")]
        [TestCase("postgresql")]
        [TestCase("multiple_db")]
        public async Task BuildHost(string jsonFileName)
        {
            //var hostBuilder = new Host.CreateDefaultBuilder();
            //var host = hostBuilder
            //    //.ConfigureAppConfiguration(builder =>
            //    //{
            //    //    builder.AddJsonFile($"configs/load_db_config/{jsonFileName}.json");
            //    //})
            //    .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
            //    .UseAutofac();
            //await host.StartAsync();
            //await host.StopAsync();
            var args = new string[1];
            CreateHostBuilder().Build().Run();
        }

        public static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddJsonFile($"configs/load_db_config/sqlite.json");
                })
                //.UseServiceProviderFactory(_ => new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
            .UseAutofac()
                ;
    }
}