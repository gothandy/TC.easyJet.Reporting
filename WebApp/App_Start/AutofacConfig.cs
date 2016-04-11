using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using System.Web.Mvc;
using Vincente.Azure.Tables;
using Vincente.Cached;
using Vincente.Config;
using Vincente.Data.Interfaces;
using Vincente.Data.Tables;
using Vincente.WebApp.App_Start;
using Vincente.WebApp.Models;
using Autofac.Integration.WebApi;
using System.Web.Http;

namespace WebApp.App_Start
{
    public class AutofaqConfig
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();
            RegisterRepositories(builder);
            var container = builder.Build();

            // MVC
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // Web API
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

        }

        private static void RegisterRepositories(ContainerBuilder builder)
        {
            var config = ConfigBuilder.Build();
            var azureStorageAccount = CloudStorageAccount.Parse(config.azureConnectionString);
            var azureTableClient = azureStorageAccount.CreateCloudTableClient();

            builderRegisterWithCache<CardTable, ICardRead, CachedCardTable>(builder, azureTableClient, config.azureCardsTableName);
            builderRegisterWithCache<TimeEntryTable, ITimeEntryRead, CachedTimeEntryTable>(builder, azureTableClient, config.azureTimeEntriesTableName);
            builderRegisterWithCache<TaskTable, ITaskRead, CachedTaskTable>(builder, azureTableClient, config.azureTasksTableName);

            builder.RegisterType<CardsByMonth>();
            builder.RegisterType<CardsByWeek>();
            builder.RegisterType<Housekeeping>();
            builder.RegisterType<InvoiceByMonth>();
            builder.RegisterType<InvoiceByWeek>();
            builder.RegisterType<TimeEntriesByMonth>();
            builder.RegisterType<TimeEntriesByWeek>();
            builder.RegisterType<ActivityByDay>();
            builder.RegisterType<ActivityBase>();

            builder.Register<NavTree>(b => NavigationConfig.GetNavigation());

            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();
            builder.RegisterApiControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();
        }

        private static void builderRegisterWithCache<T, I, C>(
            ContainerBuilder builder,
            CloudTableClient azureTableClient,
            string tableName)
        {
            var registerName = string.Format("cacheTable{0}", tableName);

            builder.RegisterType<T>()
                .Named<I>(registerName)
                .WithParameter("table", azureTableClient.GetTableReference(tableName));

            builder.RegisterType<C>()
                .As<I>()
                .WithParameter(Autofac.Core.ResolvedParameter.ForNamed<I>(registerName));
        }
    }
}