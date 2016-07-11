using Microsoft.Practices.Unity;
using RestApi.Tests.Mocks;

namespace RestApi.Tests
{
    public static class UnityConfig
    {
        public static IUnityContainer UnityContainer { get; set; }

        public static void RegisterComponents()
        {
			UnityContainer = new UnityContainer();

            UnityContainer.RegisterInstance(MockPatientContext.Object());
        }
    }
}