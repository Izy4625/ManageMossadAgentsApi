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
                    if (missions.Count() == 0)
                    {
                        Seedmission();
                    }

                }  
                
            }
            catch (Exception ex)
            {


                Console.WriteLine("didnt connect" + ex);
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
                PhotoUrl = "Url.df",
                Status = Enum.EnumSatusAgent.Inactive,
            };
        }
        private void Seedmission()
        {
            Mission mission = new Mission
            {
               
                Status = Enum.EnumSatusMissions.MissionAuthorized,
            };
        }

        public DbSet<Target> targets { get; set; } 
        public DbSet<Agent> agents { get; set; } 
        public DbSet<Mission> missions { get; set; } 
    }
}

