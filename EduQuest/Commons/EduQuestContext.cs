using EduQuest.Features.Content;
using EduQuest.Features.Course;
using EduQuest.Features.Sections;
using EduQuest.Features.User;
using Microsoft.EntityFrameworkCore;

namespace EduQuest.Commons
{
    public class EduQuestContext(DbContextOptions<EduQuestContext> options) : DbContext(options)
    {
        public DbSet<Course> Courses { get; set; }

        public DbSet<Section> Sections { get; set; }

        public DbSet<Content> Contents { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Course
            modelBuilder.Entity<Course>()
                        .HasKey(c => c.Id);

            modelBuilder.Entity<Course>()
                        .Property(c => c.Id)
                        .ValueGeneratedOnAdd();

            modelBuilder.Entity<Course>()
                        .HasOne(c => c.Educator)
                        .WithMany(u => u.CoursesCreated)
                        .HasForeignKey(c => c.EducatorId)
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Course>()
                        .HasMany(c => c.Students)
                        .WithMany(u => u.CoursesEnrolled);

            #endregion

            #region Section
            modelBuilder.Entity<Section>()
                        .HasKey(s => s.Id);

            modelBuilder.Entity<Section>()
                        .Property(s => s.Id)
                        .ValueGeneratedOnAdd();

            modelBuilder.Entity<Section>()
                        .HasOne(s => s.Course)
                        .WithMany(c => c.Sections)
                        .HasForeignKey(s => s.CourseId)
                        .OnDelete(DeleteBehavior.NoAction);
            #endregion

            #region Content
            modelBuilder.Entity<Content>()
                        .HasKey(c => c.Id);

            modelBuilder.Entity<Content>()
                        .Property(c => c.Id)
                        .ValueGeneratedOnAdd();

            modelBuilder.Entity<Content>()
                        .HasOne(c => c.Section)
                        .WithMany(s => s.Contents)
                        .HasForeignKey(c => c.SectionId)
                        .OnDelete(DeleteBehavior.NoAction);
            #endregion

            #region User
            modelBuilder.Entity<User>()
                        .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                        .Property(u => u.Id)
                        .ValueGeneratedOnAdd();

            modelBuilder.Entity<User>()
                        .HasIndex(u => u.Email)
                        .IsUnique();
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
