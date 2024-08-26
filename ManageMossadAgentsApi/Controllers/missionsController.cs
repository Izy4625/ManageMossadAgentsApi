using ManageMossadAgentsApi.Data;
using ManageMossadAgentsApi.Models;
using ManageMossadAgentsApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManageMossadAgentsApi.Enum;
using System.Text.Json;

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
               var res = _context.missions.Include(t => t.TargetId).Include(t => t.AgentId).ToArrayAsync();
                ViewAll view = new ViewAll();
             
                var json = JsonSerializer.Serialize(res);
                
                Console.WriteLine("inside GetAttacks");
                return Ok(json);
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
            _context.missions.Update(mission);
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK);
        }
        // DELETE: api/missions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMissions(int id)
        {
            var missions = await _context.missions.FindAsync(id);
            if (missions == null)
            {
                return NotFound();
            }

            _context.missions.Remove(missions);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MissionsExists(int id)
        {
            return _context.missions.Any(e => e.Id == id);
        }
    }

}

