using System.Web.Mvc;
using Microsoft.Practices.Unity;
using TableTennis.Authentication.MongoDB;
using TableTennis.Interfaces.HelperClasses;
using TableTennis.Interfaces.Repository;
using TableTennis.Models;
using Unity.Mvc4;

namespace TableTennis
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IMatchManagementRepository, MongoMatchManagement>();
            container.RegisterType<IPlayerManagementRepository, MongoPlayerManagement>();
            container.RegisterType<IRatingCalculator, RatingCalculator>();

            container.RegisterType<IAuthenticationRepository, MongoAuthenticationRepository>();
            RegisterTypes(container);

            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {

        }
    }
}