using ManageMossadAgentsApi.Data;
using ManageMossadAgentsApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ManageMossadAgentsApi.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;

namespace ManageMossadAgentsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class agentsController : ControllerBase
    {

        private readonly MossadDbContext _context;
        private readonly Service _services;
        private readonly MissionManager _missionManager;

        public agentsController(MossadDbContext context, Service service, MissionManager missionManager)
        {
            _context = context;
            _services = service;
            _missionManager =missionManager;
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
        public async Task<IActionResult> CreateAgent(Agent agent)
        {


          


           await _missionManager.HandleMissions(agent);



            Console.WriteLine("Got inside the function of creating attack");
            return StatusCode(
                StatusCodes.Status201Created,
                new { success = true, agent = agent }
                );
        }
        [HttpPut("{id}/pin")]
        public async Task<IActionResult> putpin(int id, Location location)
        {
            if (location == null)
            {
                return BadRequest();
            }
            Agent agent = await _context.agents.FindAsync(id);
            if (agent == null)
            {
                return BadRequest($"Unable to find agent by given {id}");
            }
            agent.Location = location;
            _context.Update(agent);
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status201Created, new
            {
                succes = true,
                agent = agent
            });
        }
        [HttpPut("{id}/move")]
        public async  Task<IActionResult> MoveAgent(int id, [FromBody] Direction direction)
        {
            string direct = direction.direction;
         
            Agent agent = await  _services.Movemanpan(id, direct);
            if (agent == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new
                {
                    massege = "Didn't receive anything from the function Movepawn"
                });
            }
            _context.Update(agent);
            await _context.SaveChangesAsync();
            
            
              var agents = await _context.agents.Include(t => t.Location)?.ToArrayAsync();
            var agent1 = agents.FirstOrDefault(l => l.Id == id);
            return StatusCode(StatusCodes.Status200OK, new
            {
                success = true, message = agent1
            });

        }


        }
    }
