using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ManageMossadAgentsApi.Models;
using ManageMossadAgentsApi.Data;

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
        public async Task<IActionResult> GetAttacks()
        {
            try
            {
                var attacks =  _context.targets.ToList();
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
    }
}
