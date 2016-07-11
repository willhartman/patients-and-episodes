using Microsoft.Practices.Unity;
using RestApi.Interfaces;
using RestApi.Models;
using System.Web.Http;
using Unity.WebApi;

namespace RestApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IDbContext, PatientContext>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}