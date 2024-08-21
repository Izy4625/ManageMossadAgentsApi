using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ManageMossadAgentsApi.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }



        [Range(0,1000)]
        public int x { get; set; }
        [Range(0, 1000)]
        public int y { get; set; }

        public Location(int x, int y)
        {
            this.x = x;
            this.y = y;

        }
    }
}
