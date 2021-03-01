using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository.DbData
{
    // for using Entityframework with SqlServer for Aliment Controller
    public class SqlContext : DbContext
    {
        // NuGet packages to be installed:
        // MicrosoftEntityFrameworkCore, EntityFrameworkCore.Design, EntityFrameworkCore.SqlServer, EntityFrameworkCore.Tools
        //      operations must be executed in project that has Classes that needs to be migrated to DB. 
        //      Project must be set as StartUp project
        // Open Command prompt on project folder: dotnet tool update --global dotnet-ef (update entity framework tools to last version)
        //      dotnet ef migrations add InitialCreate
        //      dotnet ef migrations remove
        //      dotnet ef database update - SAVE changes to database
        //      dotnet build - build the application
        //      dotnet run   - run the application - use Ctrl+C to STOP running the application
        public SqlContext(DbContextOptions<SqlContext> opt) : base(opt)
        {
        }

        // quick script in case you need it: CREATE TABLE [dbo].[Aliments]([Id] [int] IDENTITY(1,1) NOT NULL,[Name] [varchar](250) NOT NULL,[Line] [varchar](50) NOT NULL,[Platform] [varchar](50) NOT NULL)

        public DbSet<Aliment> Aliments { get; set; }
        //public DbSet<Cards> Cards { get; set; }
    }
}
