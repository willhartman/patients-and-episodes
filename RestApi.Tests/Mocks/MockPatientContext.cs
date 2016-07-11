using Moq;
using RestApi.Interfaces;
using RestApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RestApi.Tests.Mocks
{
    public static class MockPatientContext
    {
        public static IDbContext Object()
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

            return mockDbContext.Object;
        }

        private static void ConfigureMockDbSet<T>(Mock<DbSet<T>> mockDbSet, IQueryable<T> testData) where T : class
        {
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(testData.Provider);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(testData.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(testData.ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(testData.GetEnumerator());
        }

        private static IQueryable<Patient> GetTestPatients()
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

        private static IQueryable<Episode> GetTestEpisodes()
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
