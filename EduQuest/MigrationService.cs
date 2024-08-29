using System.Diagnostics.CodeAnalysis;
using EduQuest.Commons;
using Microsoft.EntityFrameworkCore;

public class DatabaseMigrationService
{

    [ExcludeFromCodeCoverage]
    public static void MigrateInitial(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();

        serviceScope.ServiceProvider.GetService<EduQuestContext>().Database.Migrate();
    }
}