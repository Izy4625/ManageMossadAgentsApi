using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ManageMossadAgentsApi.Models;
using ManageMossadAgentsApi.Data;
using Microsoft.EntityFrameworkCore;

namespace ManageMossadAgentsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TargetsController : ControllerBase
    {


        private readonly MossadDbContext _context;

        public TargetsController(MossadDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetTargewts()
        {
            try
            {
                var targets = await _context.agents.Include(t => t.Location)?.ToArrayAsync();
                Console.WriteLine("inside GetAttacks");
                return Ok(targets);
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
        public IActionResult CreateAttack(Target target)
        {


            _context.targets.Add(target);
            _context.SaveChanges();
            Console.WriteLine("Got inside the function of creating attack");
            return StatusCode(
                StatusCodes.Status201Created,
                new { success = true, attack = target }
                );
        }
        [HttpPut("{id}/pin")]
        public async Task<IActionResult> putpin(int id, Location location)
        {
            if (location == null)
            {
                return BadRequest();
            }
            var targets = await _context.targets.Include(t => t.Location)?.ToArrayAsync();
            var target = targets.FirstOrDefault(l => l.Id == id);
            if (target == null)
            {
                return BadRequest($"Unable to find agent by given {id}");
            }
            target.Location = location;
            _context.Update(target);
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status201Created, new
            {
                succes = true,
                agent = target
            });
        }
    }
}
