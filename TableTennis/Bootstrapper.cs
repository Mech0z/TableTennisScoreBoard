using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Practices.Unity;
using TableTennis.HelperClasses;
using TableTennis.Interfaces.HelperClasses;
using TableTennis.Interfaces.Repository;
using TableTennis.MongoDB;
using TableTennis.MongoDB.Authentication;
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
            container.RegisterInstance(typeof(IPersistentConnectionContext), GlobalHost.ConnectionManager.GetConnectionContext<ScoreConnection>());
            RegisterTypes(container);

            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {

        }
    }
}