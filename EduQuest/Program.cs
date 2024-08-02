
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Answers;
using EduQuest.Features.Articles;
using EduQuest.Features.Auth;
using EduQuest.Features.Contents;
using EduQuest.Features.CourseCategories;
using EduQuest.Features.Courses;
using EduQuest.Features.Notes;
using EduQuest.Features.Orders;
using EduQuest.Features.Payments;
using EduQuest.Features.Questions;
using EduQuest.Features.Reviews;
using EduQuest.Features.Sections;
using EduQuest.Features.Student;
using EduQuest.Features.StudentCourses;
using EduQuest.Features.Users;
using EduQuest.Features.Videos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using StudentCourseRepo = EduQuest.Entities.StudentCourseRepo;

namespace EduQuest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\""
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
            });

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey:Key"]))
                };
            });

            #region Roles
            builder.Services.AddAuthorizationBuilder()
            .AddPolicy("Educator", policy => policy.RequireRole("Educator"))
            .AddPolicy("Student", policy => policy.RequireRole("Student"))
            .AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            #endregion

            var client = new SecretClient(vaultUri: new Uri("https://eduquest-keys.vault.azure.net/"), credential: new DefaultAzureCredential());

            string storageConn = client.GetSecret("eduquest-storage").Value.Value.ToString();

            string? dbConn = builder.Environment.IsDevelopment() ? builder.Configuration.GetConnectionString("default") : client.GetSecret("eduquest-db").Value.Value.ToString();

            #region DB and Storage
            builder.Services.AddDbContext<EduQuestContext>(options =>
            {
                options.UseSqlServer(dbConn);
            });

            builder.Services.AddAzureClients(clientBuilder =>
            {
                clientBuilder.AddSecretClient(vaultUri: new Uri("https://eduquest-keys.vault.azure.net/"));
                clientBuilder.AddBlobServiceClient(storageConn);
            });

            #endregion


            #region Services
            builder.Services.AddScoped<IControllerValidator, ControllerValidator>();

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IVideoService, VideoService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IContentService, ContentService>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<ISectionService, SectionService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IArticleService, ArticleService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<INotesService, NotesService>();
            builder.Services.AddScoped<IQuestionService, QuestionService>();
            builder.Services.AddScoped<IAnswerService, AnswersService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            #endregion

            #region Repositories
            builder.Services.AddScoped<IRepository<int, User>, UserRepo>();
            builder.Services.AddScoped<IRepository<int, Order>, OrderRepo>();
            builder.Services.AddScoped<IRepository<int, Payment>, PaymentRepo>();
            builder.Services.AddScoped<IRepository<int, StudentCourseRepo>, Features.StudentCourses.StudentCourseRepo>();
            builder.Services.AddScoped<IRepository<int, CourseCategory>, CategoryRepo>();

            builder.Services.AddScoped<IAnswerRepo, AnswerRepo>();
            builder.Services.AddScoped<IArticleRepo, ArticleRepo>();
            builder.Services.AddScoped<IVideoRepo, VideoRepo>();
            builder.Services.AddScoped<ISectionRepo, SectionRepository>();
            builder.Services.AddScoped<IContentRepo, ContentRepository>();
            builder.Services.AddScoped<ICourseRepo, CourseRepository>();
            builder.Services.AddScoped<INotesRepo, NotesRepo>();
            builder.Services.AddScoped<IQuestionRepo, QuestionRepo>();
            builder.Services.AddScoped<IReviewRepo, ReviewRepo>();
            #endregion



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
