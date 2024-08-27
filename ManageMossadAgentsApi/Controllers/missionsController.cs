using ManageMossadAgentsApi.Data;
using ManageMossadAgentsApi.Models;
using ManageMossadAgentsApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManageMossadAgentsApi.Enum;
using System.Text.Json;
using System.Reflection;

namespace ManageMossadAgentsApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class missionsController : ControllerBase
    {

        private readonly MossadDbContext _context;
        private readonly MissionHandler _missionhandler;

        public missionsController(MossadDbContext context, MissionHandler missionHandler)
        {
            _context = context;
            _missionhandler = missionHandler;
        }
        [HttpGet]
        
        public async Task<ActionResult> getmissions()
        {
            try
            {
                
                List<Getsuggestion>? item = (
                from ai in _context.missions
                join al in _context.agents on ai.Agent.Id equals al.Id
                join aj in _context.targets on ai.Target.Id equals aj.Id
                
                select new Getsuggestion
                {
                   MissionId = ai.Id,
                    TargetName = aj.Name,
                    TargetNotes = aj.Position,
                    AgentNickname = al.Nickname,
                    TimeLeft = ai.MissionTimer,
                    DistanceOBetween = ai.DistanceBetween,

                }).ToList();
              

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
        [HttpPut("{id}")]
        public  async Task<IActionResult> StartMissuion(int id, ChangeMissionstatus assign)
        {
            string status = assign.status;
            if (status == null) { return BadRequest(); }
            if (status == "assigned")
            {
              bool result =  await _missionhandler.AssignMission(id);
               
                if(!(result))
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                                             
            }
            return StatusCode(StatusCodes.Status200OK); 
            
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            Mission mission =  this._context.missions.FirstOrDefault(l => l.Id == id);      
            if (mission == null) {return BadRequest();}
            var json = JsonSerializer.Serialize(mission);
            return Ok(json);

        }


        [HttpPost("update")]
        public async Task<ActionResult> UpdateMissions()
        {
           await _missionhandler.MissionUpdateHandler();
            return StatusCode(StatusCodes.Status200OK);
        }

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

