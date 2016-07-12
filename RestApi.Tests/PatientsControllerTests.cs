using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using RestApi.Controllers;
using System.Web.Http;
using System.Net;
using Microsoft.Practices.Unity;
using RestApi.Tests.Mocks;
using RestApi.Interfaces;

namespace RestApi.Tests
{
    [TestClass]
    public class PatientsControllerTests
    {
        private PatientsController _patientsController { get; set; }
        
        [TestInitialize]
        public void Initialize()
        {
            // Register components using existing DI container.
            UnityConfig.RegisterComponents();

            // Override just the IDbContext PatientContext for testing.
            UnityConfig.UnityContainer.RegisterInstance(MockPatientContext.Object());

            _patientsController = UnityConfig.UnityContainer.Resolve<PatientsController>();
        }
        
        [TestMethod]
        public void Get_RequestValidData_ReturnValidData()
        {
            var result = _patientsController.Get(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.PatientId, 1);
            Assert.IsNotNull(result.Episodes);
            Assert.AreEqual(result.Episodes.Count(), 1);
        }

        [ExpectedException(typeof(HttpResponseException))]
        [TestMethod]
        public void Get_RequestMissingData_Throws404()
        {
            try
            {
                var result = _patientsController.Get(2);
            }
            catch (HttpResponseException e)
            {
                Assert.AreEqual(e.Response.StatusCode, HttpStatusCode.NotFound);
                throw;
            }
        }
    }
}
