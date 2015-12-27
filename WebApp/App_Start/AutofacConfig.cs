using Autofac;
using Autofac.Integration.Mvc;
using Cached;
using Microsoft.WindowsAzure.Storage;
using System.Configuration;
using System.Web.Mvc;
using Vincente.Azure;
using Vincente.Azure.Converters;
using Vincente.Azure.Entities;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;
using WebApp.Models;

namespace WebApp.App_Start
{
    public class AutofaqConfig
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();
            RegisterRepositories(builder);
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private static void RegisterRepositories(ContainerBuilder builder)
        {
            var azureConnectionString = ConfigurationManager.AppSettings["azureConnectionString"];
            var azureStorageAccount = CloudStorageAccount.Parse(azureConnectionString);
            var azureTableClient = azureStorageAccount.CreateCloudTableClient();

            builder.RegisterType<JoinClient>();

            builder.RegisterType<TableClient>()
                .WithParameter("connectionString", azureConnectionString);

            builder.RegisterType<AzureTable<Card, CardEntity>>()
                .Named<ITableRead<Card>>("AzureCardTable")
                .WithParameter("table", azureTableClient.GetTableReference("Cards"))
                .WithParameter("converter", new CardConverter());

            builder.RegisterType<CachedCardTable>()
                .As<ITableRead<Card>>()
                .WithParameter(Autofac.Core.ResolvedParameter.ForNamed<ITableRead<Card>>("AzureCardTable"));

            builder.RegisterType<AzureTable<TimeEntry, TimeEntryEntity>>()
                .As<ITableRead<TimeEntry>>()
                .WithParameter("table", azureTableClient.GetTableReference("TimeEntries"))
                .WithParameter("converter", new TimeEntryConverter());
        }
    }
}