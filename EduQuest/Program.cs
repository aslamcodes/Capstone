
using EduQuest.Commons;
using EduQuest.Features.Auth;
using EduQuest.Features.Course;
using EduQuest.Features.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Diagnostics;

namespace EduQuest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("EduQuest Swagger", new OpenApiInfo { Title = "Eduquest" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."

                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                         new string[] {}
                    }
                });
            });

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            #region Context
            builder.Services.AddDbContext<EduQuestContext>(options =>
            {
                Console.WriteLine("\n\n\n\n" + builder.Configuration.GetConnectionString("Default") + "\n\n\n\n");
                Debug.WriteLine("\n\n\n\n" + builder.Configuration.GetConnectionString("Default") + "\n\n\n\n");
                options.UseSqlServer(builder.Configuration.GetConnectionString("default"));
            });

            #endregion

            #region Services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<ICourseService, CourseService>();
            #endregion

            #region Repositories
            builder.Services.AddScoped<IRepository<int, User>, UserRepo>();
            builder.Services.AddScoped<IRepository<int, Course>, CourseRepository>();
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
