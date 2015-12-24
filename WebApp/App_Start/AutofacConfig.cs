using Autofac;
using Autofac.Integration.Mvc;
using System.Configuration;
using System.Web.Mvc;
using Vincente.Azure;
using Vincente.Azure.Tables;
using Vincente.Data.Tables;
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

            builder.RegisterType<JoinClient>().InstancePerRequest();

            builder.RegisterType<TableClient>()
                .InstancePerRequest()
                .WithParameter("connectionString", azureConnectionString);

            builder.RegisterType<CardTable>().InstancePerRequest();
            builder.RegisterType<TimeEntryTable>()
                .As<ITimeEntryTable>()
                .InstancePerRequest();
        }
    }
}