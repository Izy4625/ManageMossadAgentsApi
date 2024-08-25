using ManageMossadAgentsApi.Enum;
using System.ComponentModel.DataAnnotations;

namespace ManageMossadAgentsApi.Models
{
    public class AgentsWithLocation
    {
        [Key]
        public int Id { get; set; }

        public string Nickname { get; set; }

        public string PhotoUrl { get; set; }
        public location? location { get; set; }

        public EnumSatusAgent? Status { get; set; }
        



        [Range(0, 1000)]
        public int x { get; set; }
        [Range(0, 1000)]
        public int y { get; set; }
    }
}
