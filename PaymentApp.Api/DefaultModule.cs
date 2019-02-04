using Autofac;
using PaymentApp.Core.Interfaces;
using PaymentApp.Core.Interfaces.Repositories;
using PaymentApp.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
// using System.Web.Compilation.BuildManager;

namespace PaymentApp.Api
{
    public class DefaultModule: Autofac.Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            // builder.RegisterType<UnitOfWork>().PropertiesAutowired();
            //builder.RegisterType<DbContext>().As<PaymentAppContext>()
            //        .InstancePerRequest();
            builder.RegisterType<PaymentAppContext>().InstancePerLifetimeScope();
            builder.RegisterType<PaymentAppContext>().As<DbContext>();
            builder.RegisterTypes(typeof(Repository<,>), typeof(IRepository<>));
            builder.RegisterType<UnitOfWork<PaymentAppContext>>().As<IUnitOfWork>().PropertiesAutowired().InstancePerLifetimeScope();
            // builder.RegisterType<IdentityService>().As<IIdentityService>();
           // var assemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Cast<Assembly>().ToArray(); // BuildManager.GetReferencedAssemblies().Cast<Assembly>(); //  // AppDomain.CurrentDomain.GetAssemblies();

            var myAss = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Where(a => a.Name.StartsWith("PaymentApp")).ToList();
            
            myAss.ForEach(a => Assembly.Load(a));

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

           builder.RegisterAssemblyTypes( assemblies)
                    .Where (t => t.Name.EndsWith("Repository"))
                    .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(assemblies)
                    .Where(t => t.Name.EndsWith("Service"))
                    .AsImplementedInterfaces();

        }
    }
}
