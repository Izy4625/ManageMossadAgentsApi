using ManageMossadAgentsApi.Enum;
using System.ComponentModel.DataAnnotations;


namespace ManageMossadAgentsApi.Models
{
    public class Missions
    {
        [Key]
        public int Id { get; set; }

        public Location AgentLocation { get; set; }

        public Location TargetLocation { get; set; }

        public TimeOnly? MissionTimer { get; set; }
        
        public EnumSatusMissions Status { get; set; }

    }
}
