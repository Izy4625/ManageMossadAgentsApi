using ManageMossadAgentsApi.Data;
using ManageMossadAgentsApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ManageMossadAgentsApi.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;

namespace ManageMossadAgentsApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class agentsController : ControllerBase
    {

        private readonly MossadDbContext _context;
       
        private readonly AgentHandler _agentHandler;
        private readonly OutOfrangeCheck _outOfrangeCheck;
        


        public agentsController(MossadDbContext context, AgentHandler agentHandler, OutOfrangeCheck outOfrangeCheck)
        {
            _context = context;

            _agentHandler = agentHandler;
            _outOfrangeCheck = outOfrangeCheck;



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
            if (!(_outOfrangeCheck.Range(location)))
            {
                return BadRequest();
            };
            var agents = await _context.agents.Include(t => t.location)?.ToArrayAsync();
            var agent = agents.FirstOrDefault(l => l.Id == id);
            if (agent == null)
            {
                return BadRequest($"Unable to find agent by given {id}");
            }
        
            if (agent.Status == Enum.EnumSatusAgent.Active) { return BadRequest(); }

            agent.location = location;
            _context.Update(agent);
            await _context.SaveChangesAsync();
           await Task.Run(async () =>
            {
                await _agentHandler.Handletargets(agent);// Whatever code you want in your thread
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
            if (!(_outOfrangeCheck.Range(agent.location)))
            {
                return BadRequest();
            };


            _context.Update(agent);
            await _context.SaveChangesAsync();
           
          await   Task.Factory.StartNew(async() =>
          {
              await _agentHandler.Handletargets(agent);
            });



            return StatusCode(StatusCodes.Status200OK, new
            {
                success = true, message = agent
            });

        }


        }
    }
