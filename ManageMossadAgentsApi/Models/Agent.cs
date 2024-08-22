using ManageMossadAgentsApi.Enum;
using System.ComponentModel.DataAnnotations;

namespace ManageMossadAgentsApi.Models
{
    public class Agent
    {
        [Key]
        public int Id { get; set; }

        public string Nickname { get; set; }

        public string PhotoUrl {  get; set; }
        public Location? Location { get; set; }

        public EnumSatusAgent? Status { get; set; }

    }
}
