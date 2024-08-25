using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ManageMossadAgentsApi.Models
{
    public class location
    {
        [Key]
        public int Id { get; set; }



        [Range(0,1000)]
        public int x { get; set; }
        [Range(0, 1000)]
        public int y { get; set; }

        public location(int x, int y)
        {
            this.x = x;
            this.y = y;

        }
    }
}
