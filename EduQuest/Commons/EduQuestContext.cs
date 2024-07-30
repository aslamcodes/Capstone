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
        public DbSet<CourseCategory> CourseCategories { get; set; }

        public DbSet<Note> Notes { get; set; }

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

            #region Notes
            modelBuilder.Entity<Note>().HasKey(n => n.Id);

            modelBuilder.Entity<Note>().Property(n => n.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Note>()
                        .HasOne(n => n.User)
                        .WithMany(u => u.Notes)
                        .HasForeignKey(n => n.UserId);

            modelBuilder.Entity<Note>()
                        .HasOne(n => n.Content)
                        .WithMany()
                        .HasForeignKey(n => n.ContentId);
            #endregion

            #region CourseCategory
            modelBuilder.Entity<CourseCategory>().HasKey(cc => cc.Id);

            modelBuilder.Entity<CourseCategory>().Property(cc => cc.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<CourseCategory>()
                        .HasMany(cc => cc.Courses)
                        .WithOne(c => c.CourseCategory)
                        .HasForeignKey(c => c.CourseCategoryId);


            modelBuilder.Entity<CourseCategory>().HasData(
                new CourseCategory { Id = 1, Name = "Programming", Description = "Courses on various programming languages and software development techniques." },
                new CourseCategory { Id = 2, Name = "Design", Description = "Courses on graphic design, UX/UI, and other design disciplines." },
                new CourseCategory { Id = 3, Name = "Business", Description = "Courses covering business management, entrepreneurship, and corporate strategy." },
                new CourseCategory { Id = 4, Name = "Marketing", Description = "Courses on digital marketing, advertising, and sales strategies." },
                new CourseCategory { Id = 5, Name = "Music", Description = "Courses on music theory, instrument training, and music production." },
                new CourseCategory { Id = 6, Name = "Photography", Description = "Courses on photography techniques, camera handling, and photo editing." },
                new CourseCategory { Id = 7, Name = "Health & Fitness", Description = "Courses on physical health, fitness routines, and nutrition." },
                new CourseCategory { Id = 8, Name = "Personal Development", Description = "Courses focused on personal growth, self-improvement, and life skills." },
                new CourseCategory { Id = 9, Name = "Lifestyle", Description = "Courses covering lifestyle improvements, hobbies, and general well-being." },
                new CourseCategory { Id = 10, Name = "IT & Software", Description = "Courses on IT infrastructure, software applications, and tech support." },
                new CourseCategory { Id = 11, Name = "Language", Description = "Courses on learning new languages and improving language proficiency." },
                new CourseCategory { Id = 12, Name = "Academics", Description = "Courses covering academic subjects and school-level education." },
                new CourseCategory { Id = 15, Name = "Engineering", Description = "Courses on various engineering disciplines and technical skills." },
                new CourseCategory { Id = 16, Name = "Science", Description = "Courses covering different scientific fields and research methods." },
                new CourseCategory { Id = 17, Name = "Mathematics", Description = "Courses on mathematics, from basic arithmetic to advanced calculus." },
                new CourseCategory { Id = 20, Name = "Data Science", Description = "Courses on data analysis, machine learning, and big data." },
                new CourseCategory { Id = 21, Name = "Art & Culture", Description = "Courses on various forms of art, history, and cultural studies." },
                new CourseCategory { Id = 22, Name = "Finance & Accounting", Description = "Courses on financial management, accounting principles, and investments." },
                new CourseCategory { Id = 24, Name = "Sales", Description = "Courses on sales techniques, customer relations, and sales management." },
                new CourseCategory { Id = 26, Name = "Management", Description = "Courses on management skills, leadership, and organizational behavior." },
                new CourseCategory { Id = 27, Name = "Communication", Description = "Courses on effective communication, public speaking, and interpersonal skills." },
                new CourseCategory { Id = 42, Name = "Fitness", Description = "Courses on physical fitness, exercise routines, and healthy living." }
            );

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
