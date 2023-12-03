using AuthAPIService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthAPIService.Context.Persistence
{
    public class Store(DbContextOptions<Store> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }
}
