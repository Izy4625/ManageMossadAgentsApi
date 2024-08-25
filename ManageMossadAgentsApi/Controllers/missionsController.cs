using ManageMossadAgentsApi.Data;
using ManageMossadAgentsApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManageMossadAgentsApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class missionsController : ControllerBase
    {

        private readonly MossadDbContext _context;

        public missionsController(MossadDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        
        public async Task<ActionResult> getmissions()
        {
            try
            {
                var attacks = _context.missions.ToArray();
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

        public async Task<ActionResult> createmission(Mission mission)
        {
            _context.missions.Add(mission);
            await _context.SaveChangesAsync();
            return Ok(mission);


        }
    }
}
