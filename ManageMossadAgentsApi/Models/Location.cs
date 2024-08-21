using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ManageMossadAgentsApi.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }




        public double x { get; set; }
        public double y { get; set; }

        public Location(double x, double y)
        {
            this.x = x;
            this.y = y;

        }
    }
}
