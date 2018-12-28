using Microsoft.EntityFrameworkCore;
using FileService.Model;

namespace Infrastructure
{
    public class FileContext : DbContext
    {
        public DbSet<File> Files { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            dbContextOptionsBuilder.UseSqlServer(@"Server=db;Database=FileService;User=sa;Password=Hung7442129;");
        }
        protected override void OnModelCreating(ModelBuilder ModelBuilderInstance)
        {
            ModelBuilderInstance.Entity<File>().ToTable("File");
            ModelBuilderInstance.Entity<User>().ToTable("User");
        }
        public void EnsureCreated()
        {
            base.Database.EnsureCreated();
        }
    }
}
