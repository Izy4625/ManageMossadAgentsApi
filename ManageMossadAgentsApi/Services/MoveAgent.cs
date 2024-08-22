using ManageMossadAgentsApi.Data;
using ManageMossadAgentsApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ManageMossadAgentsApi.Services
{
    public class Service
    {

        private readonly MossadDbContext _context;
       
        public Service(MossadDbContext context)
        {
            _context = context;
        }
        public async Task<Agent> Movemanpan( int id,string MoveTo)
        {
            string direction = MoveTo;
            try
            {
                
                var agents = await _context.agents.Include(t => t.Location)?.ToArrayAsync();
                var agent = agents.FirstOrDefault(l => l.Id == id);



                switch (direction)
                {

                    case "ne":
                        agent.Location.x += 1;
                        agent.Location.y += 1;
                        break;

                    case "n":
                        agent.Location.y += 1;
                        break;

                    case "sw":
                        agent.Location.x -= 1;
                        agent.Location.y -= 1;
                        break;
                    case "s":
                        agent.Location.y -= 1;
                        break;
                    case "w":
                        agent.Location.x -= 1;
                        break;
                    case "e":
                        agent.Location.x += 1;
                        break;
                    case "nw":
                        agent.Location.x -= 1;
                        agent.Location.y += 1;
                        break;
                    case "se":
                        agent.Location.x += 1;
                        agent.Location.y -= 1;
                        break;
                    default:
                        break;

                }



                return agent;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Agent agent2 = null;
            return agent2;
            


        }
    }
}
