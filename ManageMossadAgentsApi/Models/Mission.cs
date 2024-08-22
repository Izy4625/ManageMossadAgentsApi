using ManageMossadAgentsApi.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ManageMossadAgentsApi.Models
{
    public class Mission
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Agent")]
        public int AgentId { get; set; }
        [ForeignKey("Target")]
        public int TargetId { get; set; }

        public double? MissionTimer { get; set; }
        
        public EnumSatusMissions Status { get; set; }

    }
}
