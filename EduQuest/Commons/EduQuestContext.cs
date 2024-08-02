using EduQuest.Entities;
using EntityFramework.Exceptions.SqlServer;
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

        public DbSet<Question> Questions { get; set; }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<Skill> Skills { get; set; }

        public DbSet<CourseSkill> CourseSkills { get; set; }

        public DbSet<Review> Reviews { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseExceptionProcessor();
        }

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

            #region Review
            modelBuilder.Entity<Review>()
                        .HasKey(r => r.Id);

            modelBuilder.Entity<Review>()
                        .Property(r => r.Id)
                        .ValueGeneratedOnAdd();

            modelBuilder.Entity<Review>()
                        .HasOne(r => r.Course)
                        .WithMany(c => c.Reviews)
                        .HasForeignKey(r => r.CourseId);

            modelBuilder.Entity<Review>()
                        .HasOne(r => r.ReviewedBy)
                        .WithMany()
                        .HasForeignKey(r => r.ReviewedById);

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

            #region Question

            modelBuilder.Entity<Question>().HasKey(q => q.Id);

            modelBuilder.Entity<Question>().Property(q => q.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Question>()
                        .HasOne(q => q.Content)
                        .WithMany(c => c.Questions)
                        .HasForeignKey(q => q.ContentId)
                        .OnDelete(DeleteBehavior.NoAction);
            #endregion

            #region Answer
            modelBuilder.Entity<Answer>().HasKey(a => a.Id);

            modelBuilder.Entity<Answer>().Property(a => a.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Answer>()
                        .HasOne(a => a.Question)
                        .WithMany(q => q.Answers)
                        .HasForeignKey(a => a.QuestionId)
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Answer>()
                        .HasOne(a => a.AnsweredBy)
                        .WithMany()
                        .HasForeignKey(a => a.AnsweredById)
                        .OnDelete(DeleteBehavior.NoAction);
            #endregion

            #region Skill
            modelBuilder.Entity<Skill>()
                        .HasKey(s => s.Id);

            modelBuilder.Entity<Skill>()
                        .Property(s => s.Id)
                        .ValueGeneratedOnAdd();

            modelBuilder.Entity<Course>()
                        .HasMany(c => c.Skills)
                        .WithMany()
                        .UsingEntity<CourseSkill>();

            modelBuilder.Entity<Skill>().HasData(new Skill { Id = 1, Name = "HTML" },
                                                 new Skill { Id = 2, Name = "CSS" },
                                                 new Skill { Id = 3, Name = "JavaScript" },
                                                 new Skill { Id = 4, Name = "Python" },
                                                 new Skill { Id = 5, Name = "Java" },
                                                 new Skill { Id = 6, Name = "C#" },
                                                 new Skill { Id = 7, Name = "C++" },
                                                 new Skill { Id = 8, Name = "Ruby" },
                                                 new Skill { Id = 9, Name = "PHP" },
                                                 new Skill { Id = 10, Name = "SQL" },
                                                 new Skill { Id = 11, Name = "React" },
                                                 new Skill { Id = 12, Name = "Angular" },
                                                 new Skill { Id = 13, Name = "Vue" },
                                                 new Skill { Id = 14, Name = "Node.js" },
                                                 new Skill { Id = 15, Name = "Express" },
                                                 new Skill { Id = 16, Name = "Django" },
                                                 new Skill { Id = 17, Name = "Flask" },
                                                 new Skill { Id = 18, Name = "Spring" },
                                                 new Skill { Id = 19, Name = "Swift" },
                                                 new Skill { Id = 20, Name = "Kotlin" },
                                                 new Skill { Id = 21, Name = "TypeScript" },
                                                 new Skill { Id = 22, Name = "Go" },
                                                 new Skill { Id = 23, Name = "Rust" },
                                                 new Skill { Id = 24, Name = "Perl" },
                                                 new Skill { Id = 25, Name = "Scala" },
                                                 new Skill { Id = 26, Name = "Groovy" },
                                                 new Skill { Id = 27, Name = "Haskell" },
                                                 new Skill { Id = 28, Name = "MATLAB" },
                                                 new Skill { Id = 29, Name = "R" },
                                                 new Skill { Id = 30, Name = "SAS" },
                                                 new Skill { Id = 31, Name = "Assembly" },
                                                 new Skill { Id = 32, Name = "Shell Scripting" },
                                                 new Skill { Id = 33, Name = "Objective-C" },
                                                 new Skill { Id = 34, Name = "F#" },
                                                 new Skill { Id = 35, Name = "Elixir" },
                                                 new Skill { Id = 36, Name = "Clojure" },
                                                 new Skill { Id = 37, Name = "Erlang" },
                                                 new Skill { Id = 38, Name = "Dart" },
                                                 new Skill { Id = 39, Name = "Julia" },
                                                 new Skill { Id = 40, Name = "GraphQL" },
                                                 new Skill { Id = 41, Name = "Docker" },
                                                 new Skill { Id = 42, Name = "Kubernetes" },
                                                 new Skill { Id = 43, Name = "Terraform" },
                                                 new Skill { Id = 44, Name = "Ansible" },
                                                 new Skill { Id = 45, Name = "Puppet" },
                                                 new Skill { Id = 46, Name = "Chef" },
                                                 new Skill { Id = 47, Name = "Jenkins" },
                                                 new Skill { Id = 48, Name = "Travis CI" },
                                                 new Skill { Id = 49, Name = "CircleCI" },
                                                 new Skill { Id = 50, Name = "Git" },
                                                 new Skill { Id = 51, Name = "SVN" },
                                                 new Skill { Id = 52, Name = "Mercurial" },
                                                 new Skill { Id = 53, Name = "Public Speaking" },
                                                 new Skill { Id = 54, Name = "Creative Writing" },
                                                 new Skill { Id = 55, Name = "Project Management" },
                                                 new Skill { Id = 56, Name = "Graphic Design" },
                                                 new Skill { Id = 57, Name = "Cooking" },
                                                 new Skill { Id = 58, Name = "Photography" },
                                                 new Skill { Id = 59, Name = "Yoga" },
                                                 new Skill { Id = 60, Name = "Meditation" },
                                                 new Skill { Id = 61, Name = "Gardening" },
                                                 new Skill { Id = 62, Name = "Baking" },
                                                 new Skill { Id = 63, Name = "Event Planning" },
                                                 new Skill { Id = 64, Name = "First Aid" },
                                                 new Skill { Id = 65, Name = "Leadership" },
                                                 new Skill { Id = 66, Name = "Teamwork" },
                                                 new Skill { Id = 67, Name = "Negotiation" },
                                                 new Skill { Id = 68, Name = "Time Management" },
                                                 new Skill { Id = 69, Name = "Critical Thinking" },
                                                 new Skill { Id = 70, Name = "Problem Solving" },
                                                 new Skill { Id = 71, Name = "Customer Service" },
                                                 new Skill { Id = 72, Name = "Foreign Languages" },
                                                 new Skill { Id = 73, Name = "Musical Instrument" },
                                                 new Skill { Id = 74, Name = "Dance" },
                                                 new Skill { Id = 75, Name = "Fitness Training" },
                                                 new Skill { Id = 76, Name = "Interior Design" },
                                                 new Skill { Id = 77, Name = "Fashion Design" },
                                                 new Skill { Id = 78, Name = "Makeup Artistry" },
                                                 new Skill { Id = 79, Name = "Sewing" },
                                                 new Skill { Id = 80, Name = "Knitting" },
                                                 new Skill { Id = 81, Name = "Public Relations" },
                                                 new Skill { Id = 82, Name = "Marketing" },
                                                 new Skill { Id = 83, Name = "Sales" },
                                                 new Skill { Id = 84, Name = "Finance" },
                                                 new Skill { Id = 85, Name = "Accounting" },
                                                 new Skill { Id = 86, Name = "Investing" },
                                                 new Skill { Id = 87, Name = "Real Estate" },
                                                 new Skill { Id = 88, Name = "Economics" },
                                                 new Skill { Id = 89, Name = "Law" },
                                                 new Skill { Id = 90, Name = "History" },
                                                 new Skill { Id = 91, Name = "Philosophy" },
                                                 new Skill { Id = 92, Name = "Psychology" },
                                                 new Skill { Id = 93, Name = "Sociology" },
                                                 new Skill { Id = 94, Name = "Anthropology" },
                                                 new Skill { Id = 95, Name = "Political Science" },
                                                 new Skill { Id = 96, Name = "Geography" },
                                                 new Skill { Id = 97, Name = "Journalism" },
                                                 new Skill { Id = 98, Name = "Film Production" },
                                                 new Skill { Id = 99, Name = "Screenwriting" },
                                                 new Skill { Id = 100, Name = "Acting" });


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
