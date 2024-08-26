using ManageMossadAgentsApi.Data;
using ManageMossadAgentsApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ManageMossadAgentsApi.Enum;
using System.Linq.Expressions;
using Microsoft.IdentityModel.Tokens;

namespace ManageMossadAgentsApi.Services
{
   
    public class UpdateMission 
    {
        private  readonly MossadDbContext _context;
     


        public UpdateMission(MossadDbContext context)
        {
            _context = context;
           
        }
        private static List<Mission> missions = new List<Mission>();
        public async Task MissionUpdateHandler()
        {
            try
            {
                missions = await _context.missions.ToListAsync();
               
                if (missions.Count > 0)
                {

                    foreach (Mission mission in missions)
                    {
                        if (mission.Status == Enum.EnumSatusMissions.MissionInOperation)
                        {
                            Agent agent = await _context.agents.Include(t => t.location).FirstOrDefaultAsync(a => a.Id == mission.AgentId);
                            Target target = await _context.targets.Include(t => t.location).FirstOrDefaultAsync(a => a.Id == mission.TargetId);
                            double a = TargetHandler.CalculateDistance(agent.location, target.location);
                            mission.MissionTimer = a / 5;
                            string dir = GetDirection(target.location, agent.location);
                            if (dir.IsNullOrEmpty())
                            {

                                mission.Status = EnumSatusMissions.MissionCompleted;
                                agent.Status = EnumSatusAgent.Inactive;
                                target.Status = EnumStatusTarget.Eliminated;
                                target.location = null;
                                _context.missions.Update(mission);
                                _context.agents.Update(agent);
                                _context.targets.Update(target);
                             
                            }
                            else
                            {
                                DirectionDict.DirectionActions[dir](agent.location);

                            }
                            await _context.SaveChangesAsync();



                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
       

        public string GetDirection(location target, location agent)
        {
            
            int dirY = target.y - agent.y;
            int dirX = target.x - agent.x;
            string dir = "";
            if (dirY < 0) { dir += "n"; }
            if (dirY > 0) { dir += "s"; }
            if (dirX > 0) { dir += "e"; }
            if (dirX < 0) { dir += "w"; }
            Console.WriteLine(dir);
            return dir;


        }
    }
}
