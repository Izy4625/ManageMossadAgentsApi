using ManageMossadAgentsApi.Data;
using ManageMossadAgentsApi.Models;
using Microsoft.EntityFrameworkCore;
using ManageMossadAgentsApi.Enum;

namespace ManageMossadAgentsApi.Services
{
    public class TargetHandler
    {

        private  readonly MossadDbContext _context;
        private readonly object myLock = new object();


        public TargetHandler(MossadDbContext context)
        {
            _context = context;
        }
        
        private static List<Agent> _agents = new List<Agent>();

        
        public async Task Handletarget(Target target)
        {
           _agents = await _context.agents.Include(t => t.location)?.ToListAsync();
            lock (myLock)
            {
                foreach (Agent agent in _agents)
                {
                    if (agent.location == null || target.location == null)
                    {
                        continue;
                    }
                    double amount = CalculateDistance(target.location, agent.location);

                    if (amount > 200)
                    {
                        Console.WriteLine("its more then 200 no mission");

                    }
                    else if (amount < 200 && amount > 0 || amount == 200)
                    {
                        Mission missions = new Mission();
                        
                            missions.AgentId = agent.Id;
                            missions.TargetId = target.Id;
                            missions.Status = EnumSatusMissions.MissionAuthorized;
                            missions.MissionTimer = amount / 5;
                           missions.DistanceBetween = amount;
                            try
                            {
                                _context.missions.Add(missions);
                               
                            }
                            catch (Exception ex) { Console.WriteLine($"cant add new mission to the datbase" + ex); }
                        }
                    
                    else if (amount == 0)
                    {

                        Console.WriteLine("you need to kill him");
                    }


                }
            }
            await _context.SaveChangesAsync();

        }
        public static double CalculateDistance(location agentslocation, location targetlocation)
        {
            int y1 = agentslocation.y;
            int x1 = agentslocation.x;
            int y2 = targetlocation.y;
            int x2 = targetlocation.x;
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
     
    }
}
