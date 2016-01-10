using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.WindowsAzure.Storage;
using System.Configuration;
using System.Web.Mvc;
using Vincente.Azure.Tables;
using Vincente.Cached;
using Vincente.Data.Interfaces;
using Vincente.Data.Tables;
using Vincente.WebApp.App_Start;
using Vincente.WebApp.Models;

namespace WebApp.App_Start
{
    public class AutofaqConfig
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();
            RegisterRepositories(builder);
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private static void RegisterRepositories(ContainerBuilder builder)
        {

            var azureConnectionString = ConfigurationManager.AppSettings["azureConnectionString"];
            var azureStorageAccount = CloudStorageAccount.Parse(azureConnectionString);
            var azureTableClient = azureStorageAccount.CreateCloudTableClient();

            builderRegisterCardTableWithCache(builder, azureTableClient);
            builderRegisterTaskTableWithCache(builder, azureTableClient);
            builderRegisterTimeEntryTableWithCache(builder, azureTableClient);

            builder.RegisterType<CardsWithTime>();
            builder.RegisterType<Housekeeping>();
            builder.RegisterType<InvoiceData>();
            builder.RegisterType<TimeEntriesByMonth>();

            builder.Register<NavTree>(b => NavigationConfig.GetNavigation());

            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();

        }

        private static void builderRegisterCardTableWithCache(ContainerBuilder builder, Microsoft.WindowsAzure.Storage.Table.CloudTableClient azureTableClient)
        {
            builder.RegisterType<CardTable>()
                .Named<ICardRead>("AzureCardTable")
                .WithParameter("table", azureTableClient.GetTableReference("Cards"));

            builder.RegisterType<CachedCardTable>()
                .As<ICardRead>()
                .WithParameter(Autofac.Core.ResolvedParameter.ForNamed<ICardRead>("AzureCardTable"));
        }

        private static void builderRegisterTimeEntryTableWithCache(ContainerBuilder builder, Microsoft.WindowsAzure.Storage.Table.CloudTableClient azureTableClient)
        {
            builder.RegisterType<TimeEntryTable>()
                .Named<ITimeEntryRead>("AzureTimeEntryTable")
                .WithParameter("table", azureTableClient.GetTableReference("TimeEntries"));

            builder.RegisterType<CachedTimeEntryTable>()
                .As<ITimeEntryRead>()
                .WithParameter(Autofac.Core.ResolvedParameter.ForNamed<ITimeEntryRead>("AzureTimeEntryTable"));
        }


        private static void builderRegisterTaskTableWithCache(ContainerBuilder builder, Microsoft.WindowsAzure.Storage.Table.CloudTableClient azureTableClient)
        {
            builder.RegisterType<TaskTable>()
                .Named<ITaskRead>("AzureTaskTable")
                .WithParameter("table", azureTableClient.GetTableReference("Tasks"));

            builder.RegisterType<CachedTaskTable>()
                .As<ITaskRead>()
                .WithParameter(Autofac.Core.ResolvedParameter.ForNamed<ITaskRead>("AzureTaskTable"));
        }
    }
}