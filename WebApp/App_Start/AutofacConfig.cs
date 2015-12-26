using Autofac;
using Autofac.Integration.Mvc;
using Cached;
using System;
using System.Configuration;
using System.Web.Mvc;
using Vincente.Azure;
using Vincente.Azure.Tables;
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

            builder.RegisterType<JoinClient>();

            builder.RegisterType<TableClient>()
                .WithParameter("connectionString", azureConnectionString);

            builder.RegisterType<AzureCardTable>()
                .Named<ITable<Card>>("AzureCardTable");

            builder.RegisterType<CachedCardTable>()
                .As<ITable<Card>>()
                .WithParameter(Autofac.Core.ResolvedParameter.ForNamed<ITable<Card>>("AzureCardTable"))
                .WithParameter("period", new TimeSpan(0, 1, 0));

            builder.RegisterType<AzureTimeEntryTable>()
                .As<ITable<TimeEntry>>();
        }
    }
}