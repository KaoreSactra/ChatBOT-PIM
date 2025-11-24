using Microsoft.EntityFrameworkCore;
using api_back.Models;

namespace api_back.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}