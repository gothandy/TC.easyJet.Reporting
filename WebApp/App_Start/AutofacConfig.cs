using Autofac;
using Autofac.Integration.Mvc;
using System.Web.Mvc;
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
            builder.RegisterType<JoinClient>().InstancePerRequest();

            // builder.RegisterType<StudentRepository>().As<IStudentRepository>();
        }
    }
}