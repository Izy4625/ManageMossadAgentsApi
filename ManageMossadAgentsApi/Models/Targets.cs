using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManageMossadAgentsApi.Models
{
    public class Target
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Status { get; set; } 
        public Location? Location { get; set; }

        public string? Direction { get; set; }



    }
   
}
