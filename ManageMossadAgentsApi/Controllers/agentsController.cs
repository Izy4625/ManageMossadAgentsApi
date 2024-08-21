using ManageMossadAgentsApi.Data;
using ManageMossadAgentsApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ManageMossadAgentsApi.Models;

namespace ManageMossadAgentsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class agentsController : ControllerBase
    {

        private readonly MossadDbContext _context;

        public agentsController(MossadDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAgents()
        {
            try
            {
                var attacks =  _context.agents.ToList();
                Console.WriteLine("inside GetAttacks");
                return Ok(attacks);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult CreateAgent(Agent agent)
        {


            _context.agents.Add(agent);
            _context.SaveChanges();
            Console.WriteLine("Got inside the function of creating attack");
            return StatusCode(
                StatusCodes.Status201Created,
                new { success = true, agent = agent }
                );
        }
    }
}
