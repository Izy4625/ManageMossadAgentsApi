using ManageMossadAgentsApi.Data;
using ManageMossadAgentsApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ManageMossadAgentsApi.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;

namespace ManageMossadAgentsApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class agentsController : ControllerBase
    {

        private readonly MossadDbContext _context;
       
        private readonly AgentHandler _missionManager;
        


        public agentsController(MossadDbContext context, AgentHandler missionManager)
        {
            _context = context;
    
            _missionManager = missionManager;
         
        }
        [HttpGet]
        public async Task<IActionResult> GetAgents()
        {
            
            try
            {
                var agents = await _context.agents.Include(t => t.location)?.ToArrayAsync();
                Console.WriteLine("inside GetAttacks");
                return Ok(agents);
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

            _context.agents.Add(agent);
           await _context.SaveChangesAsync();
            //await _missionManager.HandleMissions(agent);
            Console.WriteLine("Got inside the function of creating attack");
            return StatusCode(
                StatusCodes.Status200OK,
                new { Id = agent.Id }
                );
        }

        [HttpPut("{id}/pin")]
        public async Task<IActionResult> putpin(int id, location location)
        {
            if (location == null)
            {
                return BadRequest();
            }
            var agents = await _context.agents.Include(t => t.location)?.ToArrayAsync();
            var agent = agents.FirstOrDefault(l => l.Id == id);
            if (agent == null)
            {
                return BadRequest($"Unable to find agent by given {id}");
            }
            if (location.y < 0 || location.x < 0 || location.y > 1000 || location.x > 1000)
            {
                return BadRequest();
            };
            if (agent.Status == Enum.EnumSatusAgent.Active) { return BadRequest(); }

            agent.location = location;
            _context.Update(agent);
            await _context.SaveChangesAsync();
           await Task.Run(async () =>
            {
                await _missionManager.Handletargets(agent);// Whatever code you want in your thread
            });


            return StatusCode(StatusCodes.Status201Created, new
            {
                succes = true,
                agent = agent
            });
        }
        [HttpPut("{id}/move")]
        public async  Task<IActionResult> MoveAgent(int id, [FromBody] Direction direction)
        {
            
            var agents = await _context.agents.Include(t => t.location)?.ToArrayAsync();
            var agent = agents.FirstOrDefault(l => l.Id == id);
            if (agent == null)
            {
                return BadRequest();
            }
        
            if (agent.location == null) { return BadRequest(string.Empty); }
            if(agent.Status == Enum.EnumSatusAgent.Active) { return BadRequest(); }
            string direct = direction.direction;
            DirectionDict.DirectionActions[direct](agent.location);

          
            if (agent == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new
                {
                    massege = "Didn't receive anything from the function Movepawn"
                });
            }
            _context.Update(agent);
            await _context.SaveChangesAsync();
           
          await   Task.Factory.StartNew(async() =>
            {
                await _missionManager.Handletargets(agent);// Whatever code you want in your thread
            });



            return StatusCode(StatusCodes.Status200OK, new
            {
                success = true, message = agent
            });

        }


        }
    }
