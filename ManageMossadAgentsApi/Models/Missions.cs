using ManageMossadAgentsApi.Enum;
using System.ComponentModel.DataAnnotations;


namespace ManageMossadAgentsApi.Models
{
    public class Missions
    {
        [Key]
        public int Id { get; set; }

        public int AgentId { get; set; }

        public int TargetId { get; set; }

        public double? MissionTimer { get; set; }
        
        public EnumSatusMissions Status { get; set; }

    }
}
