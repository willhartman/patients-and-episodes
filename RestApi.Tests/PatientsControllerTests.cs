using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestApi.Interfaces;
using RestApi.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using RestApi.Controllers;
using System.Web.Http;
using System.Net;

namespace RestApi.Tests
{
    [TestClass]
    public class PatientsControllerTests
    {
        private PatientsController _patientsController { get; set; }
        
        [TestInitialize]
        public void Initialize()
        {
            // Mock the DbContext.
            var mockDbContext = new Mock<IDbContext>();

            // Mock the DbSets.
            var mockPatientDbSet = new Mock<DbSet<Patient>>();
            var mockEpisodesDbSet = new Mock<DbSet<Episode>>();

            // Configure mock DbSets IQueryable to use test entities.
            ConfigureMockDbSet(mockPatientDbSet, GetTestPatients());
            ConfigureMockDbSet(mockEpisodesDbSet, GetTestEpisodes());

            // Add mock DbSets to mock DbContext.
            mockDbContext.Setup(x => x.Set<Patient>()).Returns(mockPatientDbSet.Object);
            mockDbContext.Setup(x => x.Set<Episode>()).Returns(mockEpisodesDbSet.Object);

            // And finally the PatientsController.
            _patientsController = new PatientsController(mockDbContext.Object);
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

        private void ConfigureMockDbSet<T>(Mock<DbSet<T>> mockDbSet, IQueryable<T> testData) where T : class
        {
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(testData.Provider);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(testData.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(testData.ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(testData.GetEnumerator());
        }

        private IQueryable<Patient> GetTestPatients()
        {
            return new List<Patient>
                {
                    new Patient
                        {
                            DateOfBirth = new DateTime(1972, 10, 27),
                            FirstName = "Millicent",
                            PatientId = 1,
                            LastName = "Hammond",
                            NhsNumber = "1111111111"
                        }
                }.AsQueryable();
        }

        private IQueryable<Episode> GetTestEpisodes()
        {
            return new List<Episode>
                {
                    new Episode
                        {
                            AdmissionDate = new DateTime(2014, 11, 12),
                            Diagnosis = "Irritation of inner ear",
                            DischargeDate = new DateTime(2014, 11, 27),
                            EpisodeId = 1,
                            PatientId = 1
                        }
                }.AsQueryable();
        }
        
    }
}
