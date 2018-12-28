using System;
using Microsoft.EntityFrameworkCore;
using OnlineReading.API.Models;

namespace OnlineReading.API.Infrastructure
{
    public class StoryContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<StoryTag> StoryTags { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            dbContextOptionsBuilder.UseSqlServer(@"Server=db1;Database=StoryService;User=sa;Password=Hung7442129;");
        }

        protected override void OnModelCreating(ModelBuilder ModelBuilderInstance)
        {
            ModelBuilderInstance.Entity<User>().ToTable("User");
            ModelBuilderInstance.Entity<Category>().ToTable("Category");
            ModelBuilderInstance.Entity<Tag>().ToTable("Tag");
            ModelBuilderInstance.Entity<Story>().ToTable("Story");
            ModelBuilderInstance.Entity<StoryTag>().ToTable("StoryTag");
        }
        public void EnsureCreated()
        {
            base.Database.EnsureCreated();
        }
    }
}
