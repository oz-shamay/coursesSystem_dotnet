using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace coursesSystem.Models
{
    public static class SeedData
    {
        public static void SeedDatabase(Context context)
        {
            context.Database.Migrate();
        }
    }
}