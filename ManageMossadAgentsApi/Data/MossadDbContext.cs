using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ManageMossadAgentsApi.Models;

namespace ManageMossadAgentsApi.Data
{
    public class MossadDbContext: DbContext
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
                        Seed();
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {


                Console.WriteLine("didnt connect");
            }
        }


        private void Seed()
        {
            Target target = new Target
            {
                Name = "Ismael",
                Position = "Recruit To HamasAlquada Squad",
                Status = "Alive"
            };
        }

        public DbSet<Target> targets { get; set; }
    }
}

