using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ManageMossadAgentsApi.Models;
using ManageMossadAgentsApi.Data;
using Microsoft.EntityFrameworkCore;
using ManageMossadAgentsApi.Services;
using System.Text.Json;

namespace ManageMossadAgentsApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TargetsController : ControllerBase
    {


        private readonly MossadDbContext _context;
        private readonly TargetHandler _targetHandler;

        public TargetsController(MossadDbContext context, TargetHandler targetHandler)
        {
            _context = context;
            _targetHandler = targetHandler;
        }
        [HttpGet]
        public async Task<IActionResult> GetTargewts()
        {
            try
            {
                var targets = await _context.targets.Include(t => t.location)?.ToArrayAsync();
                var json = JsonSerializer.Serialize(targets);

                Console.WriteLine("inside GetAttacks");
                return Ok(json);
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
        public async Task<IActionResult> CreateTarget(Target target)
        {


            _context.targets.Add(target);
            await _context.SaveChangesAsync();
            Console.WriteLine("Got inside the function of creating attack");
            return StatusCode(
                StatusCodes.Status201Created,
                new {Id = target.Id }
                );
        }
        [HttpPut("{id}/pin")]
        public async Task<IActionResult> putpin(int id, location location)
        {
            if (location == null)
            {
                return BadRequest();
            }
            var targets = await _context.targets.Include(t => t.location)?.ToArrayAsync();
            var target = targets.FirstOrDefault(l => l.Id == id);
            if (target == null)
            {
                return BadRequest($"Unable to find agent by given {id}");
            }
            target.location = location;
            _context.Update(target);
            await _context.SaveChangesAsync();
           await Task.Run(async () =>
            {
                await _targetHandler.HandleAgents(target);// Whatever code you want in your thread
            });
            return StatusCode(StatusCodes.Status201Created);
            
        }
        [HttpPut("{id}/move")]
        public async Task<IActionResult> MoveAgent(int id, [FromBody] Direction direction)
        {
            
            var targets = await _context.targets.Include(t => t.location)?.ToArrayAsync();
            var target = targets.FirstOrDefault(l => l.Id == id);
            if (target == null) { return BadRequest(); }    
            if(target.location == null) { return BadRequest (string.Empty); }
            string direct = direction.direction;
            DirectionDict.DirectionActions[direct](target.location);


            if (target == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new
                {
                    massege = "Didn't receive anything from the function Movepawn"
                });
            }
            _context.Update(target);
           await Task.Factory.StartNew(async () =>
            {
                await _targetHandler.HandleAgents(target);// Whatever code you want in your thread
            });
            await _context.SaveChangesAsync();



            return StatusCode(StatusCodes.Status200OK);
           

        }
    }
}
