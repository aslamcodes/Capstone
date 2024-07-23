using EduQuest.Features.Course;
using EduQuest.Features.User;
using Microsoft.EntityFrameworkCore;

namespace EduQuest.Commons
{
    public class EduQuestContext(DbContextOptions<EduQuestContext> options) : DbContext(options)
    {
        public DbSet<Course> Courses { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Course
            modelBuilder.Entity<Course>().HasKey(c => c.Id);

            #endregion

            #region User
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            #endregion

            #region Enum Conversion 
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var enumProperties = entityType.ClrType.GetProperties()
                    .Where(p => p.PropertyType.IsEnum);

                foreach (var property in enumProperties)
                {
                    modelBuilder.Entity(entityType.Name)
                        .Property(property.Name)
                        .HasConversion<string>();
                }
            }
            #endregion
        }
    }
}
