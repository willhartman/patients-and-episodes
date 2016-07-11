using System.Linq;
using System.Net;
using System.Web.Http;
using RestApi.Models;
using RestApi.Interfaces;

namespace RestApi.Controllers
{
    public class PatientsController : ApiController
    {
        private readonly IDbContext _dbContext;

        public PatientsController(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public Patient Get(int patientId)
        {
            var patientsAndEpisodes =
                from p in _dbContext.Set<Patient>()
                join e in _dbContext.Set<Episode>() on p.PatientId equals e.PatientId
                where p.PatientId == patientId
                select new {p, e};

            if (patientsAndEpisodes.Any())
            {
                var first = patientsAndEpisodes.First().p;
                first.Episodes = patientsAndEpisodes.Select(x => x.e).ToArray();
                return first;
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }
    }
}