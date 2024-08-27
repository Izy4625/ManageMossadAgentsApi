using ManageMossadAgentsApi.Models;

namespace ManageMossadAgentsApi.Services
{
    public class OutOfrangeCheck
    {
        public bool Range(location loc)
        {
            bool res = true;

            if (loc.y < 0 || loc.y > 1000 || loc.x < 0 || loc.x > 1000)
            {
                res = false;
                return res;
            }
            return res;
        }
    }
}
