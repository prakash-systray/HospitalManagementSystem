using Autofac;
using Autofac.Integration.Mvc;
using HospitalManagementSystem.Core.Interface;
using HospitalManagementSystem.Data.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HospitalManagementSystem.App_Start
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["HMS"].ConnectionString;

            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterFilterProvider();
            builder.RegisterSource(new ViewRegistrationSource());

            //builder.Register(c => new DashBoardRepository(connectionString)).As<IDashBoardRepository>().InstancePerLifetimeScope();
            builder.Register(c => new DashBoardRepository(connectionString)).As<IDashBoardRepository>().InstancePerLifetimeScope();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container)); //Set the MVC DependencyResolver 
        }




    }
}