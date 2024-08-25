using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ManageMossadAgentsApi.Enum;

namespace ManageMossadAgentsApi.Models
{
    public class Target
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public string PhotoUrl { get; set; }
        public string Position { get; set; }
        public EnumStatusTarget? Status { get; set; } 
        public location? location { get; set; }
     
    }
   
}
