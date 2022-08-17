using ElasticSearchDemo.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nest;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticSearchDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {

            }
        }
        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //  Host.CreateDefaultBuilder(args)
        //     .ConfigureServices((hostcontext, services) =>
        //     {
        //         services.AddSingleton<IElasticClient>(sp =>
        //         {
        //             var config = sp.GetRequiredService<IConfiguration>();
        //             var settings = new ConnectionSettings(new Uri(config["ELKConfiguration:Uri"])).
        //             PrettyJson().DefaultIndex("Product").
        //             DefaultMappingFor<Product>(p => p.Ignore(p => p.Price).Ignore(p => p.Id)); ;
        //             return new ElasticClient(settings);

        //         });

        //     });

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .UseSerilog(configureLogger: (context, configuration) =>
                  {
                      configuration.Enrich.FromLogContext()
                                   .Enrich.WithMachineName()
                                   .WriteTo.Console()
                                   .WriteTo.Elasticsearch(
                          new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(node: new Uri(context.Configuration["ELKConfiguration:Uri"]))
                          {

                              IndexFormat = $"{context.Configuration["ApplicationName"]}.logs.{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(oldValue: ".", newValue: "-")}-{DateTime.UtcNow:yyyy-MM}",
                              AutoRegisterTemplate = true
                              //NumberOfReplicas = 1,
                              //NumberOfShards = 2

                          })
                                   .Enrich.WithProperty(name: "Environment", context.HostingEnvironment.EnvironmentName)
                                   .ReadFrom.Configuration(context.Configuration);

                  })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
