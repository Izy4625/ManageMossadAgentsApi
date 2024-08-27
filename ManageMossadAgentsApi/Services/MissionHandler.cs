using ManageMossadAgentsApi.Data;
using ManageMossadAgentsApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ManageMossadAgentsApi.Enum;
using System.Linq.Expressions;
using Microsoft.IdentityModel.Tokens;

namespace ManageMossadAgentsApi.Services
{
   
    public class MissionHandler
    {

      
        private  readonly MossadDbContext _context;
    
        public MissionHandler(MossadDbContext context)
        {
            _context = context;
           
        }
       

       
            

              
            
            
            
            

        
        public async Task MissionUpdateHandler()
        {
            try
            {
                 var missions = await _context.missions.Include(t => t.Target)?.Include(t => t.Agent)?.Include(t => t.Agent.location)?.Include(t => t.Target.location)?.
                     ToListAsync();


                if (missions != null)
                {

                    foreach (Mission mission in missions)
                    {
                        if (mission.Status == Enum.EnumSatusMissions.MissionInOperation)
                        {
                      

                            double? a = TargetHandler.CalculateDistance(mission.Agent.location, mission.Target.location);

                            mission.MissionTimer = a / 5;
                            string dir = GetDirection(mission.Target.location, mission.Agent.location);
                            if (dir.IsNullOrEmpty())
                            {
                                _context.missions.Update(EliminateTarget(mission));                             
                            }
                            else
                            {
                                DirectionDict.DirectionActions[dir](mission.Agent.location);

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
        public async Task<bool> AssignMission(int id)
        {
            var missions = await _context.missions.Include(t => t.Target)?.Include(t => t.Agent)?.Include(t => t.Agent.location)?.Include(t => t.Target.location)?.
                     ToListAsync();
            bool res = false;
            var mission = missions.FirstOrDefault(l => l.Id == id);
            if (mission == null) { return res; };

            if (mission.Target.Status == EnumStatusTarget.Eliminated || mission.Target.Status == EnumStatusTarget.taken) { return res; }
            if (mission.Agent.Status == EnumSatusAgent.Active) { return res; }

            mission.Status = EnumSatusMissions.MissionInOperation;
            mission.Agent.Status = EnumSatusAgent.Active;
            mission.Target.Status = EnumStatusTarget.taken;
            _context.Entry(mission).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            res = true;
            return res;
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
        public Mission EliminateTarget(Mission? mission)
        {
            mission.Status = EnumSatusMissions.MissionCompleted;
            mission.Agent.Status = EnumSatusAgent.Inactive;
            mission.Target.Status = EnumStatusTarget.Eliminated;
            mission.Target.location = null;
            return mission;

        }
    }
}
