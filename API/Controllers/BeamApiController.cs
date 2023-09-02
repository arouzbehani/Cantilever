using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cantilever;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BeamApiController : ControllerBase
    {

        private readonly ILogger<BeamApiController> _logger;

        public BeamApiController(ILogger<BeamApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetDisplacements")]
        public IList<double> Get(int matId=1,int secId=1,double force=1000,double length=3000, int meshNum=10)
        {
            try
            {
            var beam = new BeamElement(length, matId,secId, force);
            return beam.Displacements(meshNum);

            }
            catch (Exception exc)
            {

                return new List<double> { 0};
            }

        }
    }
}
