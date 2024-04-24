using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using whsodergrenMIS421FinalProject.Models;

namespace whsodergrenMIS421FinalProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<whsodergrenMIS421FinalProject.Models.Category> Category { get; set; } = default!;
        public DbSet<whsodergrenMIS421FinalProject.Models.Transaction> Transaction { get; set; } = default!;
    }
}
