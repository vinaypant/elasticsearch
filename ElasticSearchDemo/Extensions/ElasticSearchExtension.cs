using ElasticSearchDemo.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;

namespace ElasticSearchDemo.Extensions
{
    public static class ElasticSearchExtension
    {

        public static void AddElasticSearch(this IServiceCollection svc, IConfiguration configuration)
        {
            try
            {
                var ff = DateTime.UtcNow;
                var url = configuration["ELKConfiguration:Uri"];
                var defaultIndex = configuration["ELKConfiguration:Index"];

                var settings = new ConnectionSettings(new Uri(url)).PrettyJson().DefaultIndex(defaultIndex);
                //table that you want to create
                AddDefaultMappings(settings);

                var elasticClient = new ElasticClient(settings);
                svc.AddSingleton<IElasticClient>(elasticClient);

                CreateIndex(elasticClient, defaultIndex);
            }
            catch (Exception ex)
            {

            }

        }
        private static void AddDefaultMappings(ConnectionSettings connectionSettings)
        {
            connectionSettings.DefaultMappingFor<Product>(p => p.Ignore(p => p.Price).Ignore(p => p.Id));
            //connectionSettings.DefaultMappingFor<Employee>(p=>p.ma)
        }

        private static void CreateIndex(IElasticClient client, string index)
        {
            client.Indices.Create(index,
                i => i.Map<Product>(x => x.AutoMap()));

            client.Indices.Create("employee",
               i => i.Map<Employee>(x => x.AutoMap()))  ;
            

        }
    }
}
