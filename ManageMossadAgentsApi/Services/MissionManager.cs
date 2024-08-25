using ManageMossadAgentsApi.Data;
using ManageMossadAgentsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ManageMossadAgentsApi.Services
{
    public class MissionManager
    {

        private  readonly MossadDbContext _context;
     
        

        
        public MissionManager(MossadDbContext context)
        {
            _context = context;
        }
        private static List<Target> _targets = new List<Target>();
        private static List<Agent> _agents = new List<Agent>();
        public  async Task HandleMissions(Agent agent)
        {

            _context.agents.Add(agent);
            _context.SaveChanges();
            var agents = await _context.agents.Include(t => t.Location)?.ToArrayAsync();
            var agent1 = agents.FirstOrDefault(l => l.Id == 7);


            var targets = await _context.agents.Include(t => t.Location)?.ToArrayAsync();


            foreach (var target in targets)
                {
                    double amount = CalculateDistance(target.Location, agent1.Location);

                    if (amount > 200)
                    {
                        Console.WriteLine("its more then 200 no mission");

                    }
                    else if (amount < 200 && amount > 0 || amount == 200 && amount > 0)
                    {
                        Mission missions = new Mission();
                        {
                            missions.AgentId = 7;
                            missions.TargetId = target.Id;
                            missions.Status = 0;
                            missions.MissionTimer = amount / 5;
                        try
                        {
                            _context.missions.Add(missions);
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception ex) { Console.WriteLine($"cant add new mission to the datbase" + ex); }
                        }
                    }
                    else if (amount == 0)
                    {

                        Console.WriteLine("you need to kill him");
                    }
                

            }
            //public void UpdaetMissions()
            //{
            //    var _missions = _context.missions.ToList();
            //    foreach( Mission mission in _missions)
            //    {
            //        if(mission.Status == Enum.EnumSatusMissions.MissionInOperation)
            //        {
                        
                            
            //        }
            //    }
            //}
            

          



        }
        public double CalculateDistance(Location agentslocation, Location targetlocation)
        {
            int y1 = agentslocation.y;
            int x1 = agentslocation.x;
            int y2 = targetlocation.y;
            int x2 = targetlocation.x;
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
     
    }
}
