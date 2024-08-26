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
                
                List<Getsuggestion>? item = (
                from ai in _context.missions
                join al in _context.agents on ai.AgentId equals al.Id
                join aj in _context.targets on ai.TargetId equals aj.Id
                


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
        public  async Task<IActionResult> GetmissionDetails(int id, ChangeMissionstatus assign)
        {
            string status = assign.status;
            if (status == null) { return BadRequest(); }
            if (status == "assigned")
            {
               
               var missions = await _context.missions.Include(t => t.TargetId).Include(t => t.AgentId).ToArrayAsync();
                var mission = missions.FirstOrDefault(l => l.Id == id);
                ViewAll view = new ViewAll();

                if (mission == null) { return BadRequest(); }
                
                mission.Status = EnumSatusMissions.MissionInOperation;
                _context.Entry(mission).State = EntityState.Modified;
               await _context.SaveChangesAsync();
            }
            return StatusCode(StatusCodes.Status200OK); 
            
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            Mission mission =  this._context.missions.FirstOrDefault(l => l.Id == id);
            //Mission mission = await _context.missions.FindAsync(id);
            if (mission == null) {return BadRequest();}
            var json = JsonSerializer.Serialize(mission);
            return Ok(json);

        }


        [HttpPost("update")]
        public async Task<ActionResult> UpdateMissions()
        {
           await _updatemission.MissionUpdateHandler();
            return StatusCode(StatusCodes.Status200OK);
        }
        //[HttpPut("{Id}")]
        //public async Task<IActionResult> StartMission(int Id)
        //{
        //    Mission mission = await _context.missions.FirstOrDefaultAsync(x => x.Id == Id);
        //    if (mission == null) { return BadRequest(); }
        //    mission.Status = Enum.EnumSatusMissions.MissionInOperation;
        //    _context.missions.Update(mission);
        //    await _context.SaveChangesAsync();
        //    return StatusCode(StatusCodes.Status200OK);
        //}
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

