using Microsoft.Practices.Unity;
using RestApi.Interfaces;
using RestApi.Models;
using System.Web.Http;
using Unity.WebApi;

namespace RestApi
{
    public static class UnityConfig
    {
        public static IUnityContainer UnityContainer { get; set; }

        public static void RegisterComponents()
        {
			UnityContainer = new UnityContainer();

            UnityContainer.RegisterType<IDbContext, PatientContext>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(UnityContainer);
        }
    }
}