using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManageMossadAgentsApi.Data;
using ManageMossadAgentsApi.Models;

namespace ManageMossadAgentsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class missionsController : ControllerBase
    {
        private readonly ManageMossadAgentsApiContext _context;

        public missionsController(ManageMossadAgentsApiContext context)
        {
            _context = context;
        }

        // GET: api/missions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mission>>> GetMissions()
        {
            return await _context.Missions.ToListAsync();
        }

        // GET: api/missions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Mission>> GetMissions(int id)
        {
            var missions = await _context.Missions.FindAsync(id);

            if (missions == null)
            {
                return NotFound();
            }

            return missions;
        }

        // PUT: api/missions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMissions(int id, Mission missions)
        {
            if (id != missions.Id)
            {
                return BadRequest();
            }
            missions.Status = 0;
            _context.Entry(missions).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MissionsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/missions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Mission>> MissionsUpdate(Mission missions)
        {
            _context.Missions.Add(missions);
            await _context.SaveChangesAsync();

            return StatusCode(
               StatusCodes.Status201Created,
               new { success = true, mission = missions }
               );
        }

        // DELETE: api/missions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMissions(int id)
        {
            var missions = await _context.Missions.FindAsync(id);
            if (missions == null)
            {
                return NotFound();
            }

            _context.Missions.Remove(missions);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MissionsExists(int id)
        {
            return _context.Missions.Any(e => e.Id == id);
        }
    }
}
