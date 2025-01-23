
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Projectapi.Models
{
    public class appContext :IdentityDbContext<ApplicationUser>
    {
        public DbSet<Department > Department { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public appContext(DbContextOptions<appContext>  options) : base(options)
        { }
  
    }
}
