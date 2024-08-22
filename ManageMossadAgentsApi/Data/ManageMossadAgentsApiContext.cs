using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ManageMossadAgentsApi.Models;

namespace ManageMossadAgentsApi.Data
{
    public class ManageMossadAgentsApiContext : DbContext
    {
        public ManageMossadAgentsApiContext (DbContextOptions<ManageMossadAgentsApiContext> options)
            : base(options)
        {
        }

        public DbSet<ManageMossadAgentsApi.Models.Agent> Agent { get; set; } = default!;
        public DbSet<ManageMossadAgentsApi.Models.Missions> Missions { get; set; } = default!;
    }
}
