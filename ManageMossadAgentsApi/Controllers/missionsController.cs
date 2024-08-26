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
                
                Getsuggestion? item = (
                from ai in _context.missions
                join al in _context.agents on ai.AgentId equals al.Id
                join aj in _context.targets on ai.TargetId equals aj.Id
                


                select new Getsuggestion
                {
                    TargetName = aj.Name,
                    TargetNotes = aj.Position,
                    AgentNickname = al.Nickname,
                   

                }).FirstOrDefault();
                //Mission[] res = _context.missions.Include(t => t.TargetId).Include(t => t.AgentId).ToArrayAsync();
                //ViewAll view = new ViewAll();

             
                var json = JsonSerializer.Serialize(item);
                
                Console.WriteLine("inside GetAttacks");
                return Ok(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetmissionDetails(int id)
        {
            
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

