using AuthAPIService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthAPIService.Context.Persistence
{
    public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }
}
