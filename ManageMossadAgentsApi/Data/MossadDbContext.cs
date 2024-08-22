using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ManageMossadAgentsApi.Models;

namespace ManageMossadAgentsApi.Data
{
    public class MossadDbContext : DbContext
    {
        public MossadDbContext(DbContextOptions<MossadDbContext> options)
       : base(options)
        {
            try
            {
                if (Database.EnsureCreated())
                {

                    if (targets.Count() == 0)
                    {
                        Seedtarget();
                    }
                    if (agents.Count() == 0)
                    {
                        Seedagent();
                    }
                }
                   
                
            }
            catch (Exception ex)
            {


                Console.WriteLine("didnt connect");
            }
        }


        private void Seedtarget()
        {
            Target target = new Target
            {
                Name = "Ismael",
                Position = "Recruit To HamasAlquada Squad",
                Status = Enum.EnumStatusTarget.Alive,
            };



        }
        private void Seedagent()
        {
            Agent agent = new Agent
            {
                Nickname = "Islam",
                photo = "Url.df",
                Status = Enum.EnumSatusAgent.Inactive,
            };
        }

        public DbSet<Target> targets { get; set; } = default;
        public DbSet<Agent> agents { get; set; } = default;
    }
}

