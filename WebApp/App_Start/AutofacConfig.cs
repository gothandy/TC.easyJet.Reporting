using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.WindowsAzure.Storage;
using System.Configuration;
using System.Web.Mvc;
using Vincente.Azure.Tables;
using Vincente.Cached;
using Vincente.Data.Interfaces;
using Vincente.Data.Tables;
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

            builder.Register<NavTree>(b => GetRootNode());

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

        private static NavTree GetRootNode()
        {
            var home =

                new NavTree("Home", "Index", "Default")
                {
                    new NavTree("Invoice", "List", "Invoice"),
                    new NavTree("WIP", "ByList", "Wip"),
                    new NavTree("Users", "Summary", "User"),

                    new NavTree("Data", "Index", "Data")
                    {
                        new NavTree("Budget Control", "BudgetControl", "Data", "1"),
                        new NavTree("SDN", "SDN", "Data", "1"),
                        new NavTree("Cards", "Cards", "Data", "2"),
                        new NavTree("Tasks", "Tasks", "Data", "2"),
                        new NavTree("Time Entries", "TimeEntries", "Data", "2")
                    },

                    new NavTree("Errors", "Summary", "Error")
                };

            return home;
        }
    }
}