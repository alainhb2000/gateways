using Microsoft.EntityFrameworkCore;

namespace Gateways
{
    public class GatewaysContext : DbContext
    {
        public GatewaysContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Gateway> Gateways { get; set; }
    }
}
