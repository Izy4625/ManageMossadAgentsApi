using ManageMossadAgentsApi.Data;
using ManageMossadAgentsApi.Models;
using ManageMossadAgentsApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManageMossadAgentsApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class missionsController : ControllerBase
    {

        private readonly MossadDbContext _context;
        private readonly UpdateMission _updatemission;

        public missionsController(MossadDbContext context, UpdateMission updateMission)
        {
            _context = context;
            _updatemission = updateMission;
        }
        [HttpGet]
        
        public async Task<ActionResult> getmissions()
        {
            try
            {
                var attacks = await _context.missions.ToArrayAsync();
                Console.WriteLine("inside GetAttacks");
                return Ok(attacks);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

    
        [HttpPost("update")]
        public async Task<ActionResult> UpdateMissions()
        {
           await _updatemission.MissionUpdateHandler();
            return StatusCode(StatusCodes.Status200OK);
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> StartMission(int Id)
        {
            Mission mission = await _context.missions.FirstOrDefaultAsync(x => x.Id == Id);
            if (mission == null) { return BadRequest(); }
            mission.Status = Enum.EnumSatusMissions.MissionInOperation;
            return StatusCode(StatusCodes.Status200OK);
        }


    }
}
