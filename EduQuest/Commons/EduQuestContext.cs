using EduQuest.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduQuest.Commons
{
    public class EduQuestContext(DbContextOptions<EduQuestContext> options) : DbContext(options)
    {
        public DbSet<Course> Courses { get; set; }

        public DbSet<Section> Sections { get; set; }

        public DbSet<Content> Contents { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }

        public DbSet<Video> Videos { get; set; }

        public DbSet<Article> Articles { get; set; }

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
                        .WithMany(u => u.CoursesEnrolled)
                        .UsingEntity<StudentCourse>();

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

            modelBuilder.HasSequence<int>("SectionOrders")
                        .StartsAt(1)
                        .IncrementsBy(1);

            modelBuilder.Entity<Section>()
                        .Property(o => o.OrderId)
                        .HasDefaultValueSql("NEXT VALUE FOR SectionOrders");
            #endregion

            #region Content
            modelBuilder.Entity<Content>()
                        .HasKey(c => c.Id);

            modelBuilder.Entity<Content>()
                        .Property(c => c.Id)
                        .ValueGeneratedOnAdd();

            modelBuilder.HasSequence<int>("ContentOrders")
                        .StartsAt(1)
                        .IncrementsBy(1);

            modelBuilder.Entity<Content>()
                        .Property(o => o.OrderId)
                        .HasDefaultValueSql("NEXT VALUE FOR ContentOrders");

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

            #region Order
            modelBuilder.Entity<Order>()
                        .HasKey(o => o.Id);

            modelBuilder.Entity<Order>()
                        .Property(o => o.Id)
                        .ValueGeneratedOnAdd();

            modelBuilder.Entity<Order>()
                        .HasOne(o => o.OrderedCourse)
                        .WithMany(u => u.Orders)
                        .HasForeignKey(o => o.OrderedCourseId);

            modelBuilder.Entity<Order>()
                        .HasOne(o => o.OrderedUser)
                        .WithMany(u => u.CoursesOrdered);
            #endregion

            #region Payments
            modelBuilder.Entity<Payment>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();


            modelBuilder.Entity<Payment>()
                      .HasOne(payment => payment.Order)
                      .WithOne(order => order.Payment)
                      .HasForeignKey<Payment>(p => p.OrderId);
            #endregion

            #region Video
            modelBuilder.Entity<Video>().HasKey(v => v.Id);

            modelBuilder.Entity<Video>().Property(v => v.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Video>()
                        .HasOne(v => v.Content)
                        .WithOne(c => c.Video)
                        .HasForeignKey<Video>(v => v.ContentId);
            #endregion

            #region Article
            modelBuilder.Entity<Article>().HasKey(a => a.Id);

            modelBuilder.Entity<Article>().Property(a => a.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Article>()
                        .HasOne(a => a.Content)
                        .WithOne(c => c.Article)
                        .HasForeignKey<Article>(a => a.ContentId);

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
