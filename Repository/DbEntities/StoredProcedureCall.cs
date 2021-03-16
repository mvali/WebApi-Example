using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Repository.DbEntities
{
    // Select data with a stored procedure with Entity Framework Core 5

    public class StoredProcedureCall
    {
        public StoredProcedureCall()
        {
        }

    }

    //  dotnet ef migrations add spGetGuestsForDate  //  add a database migration with an appropriate SQL.
    //                                               //  This will generate a migration, that we can put our SQL into
    public partial class spGetGuestsForDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"
            IF OBJECT_ID('GetGuestsForDate', 'P') IS NOT NULL
            DROP PROC GetGuestsForDate
            GO

            CREATE PROCEDURE [dbo].[GetGuestsForDate]
                @StartDate varchar(20)
            AS
            BEGIN
                SET NOCOUNT ON;
                SELECT p.Forename, p.Surname, p.TelNo, r.[From], r.[To], ro.Number As RoomNumber
                FROM Profiles p
                JOIN Reservations r ON p.ReservationId = p.ReservationId
                JOIN Rooms ro ON r.RoomId = ro.Id
                WHERE CAST([From] AS date) = CONVERT(date, @StartDate, 105)
            END";

            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROC GetGuestsForDate");
        }
    }

    // Selecting data with a stored procedure
    [Keyless]   //  Keyless entities have most of the mapping capabilities as normal entities,
                //  they are not tracked for changes in the DbContext.
                //  not be able to perform insert, update, or delete on this entity
    public class GuestArrival
    {
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string TelNo { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int RoomNumber { get; set; }
    }

    //  need to add a DbSet to our PrimeDbContext
    public class PrimeDbContext : DbContext
    {
        public PrimeDbContext(DbContextOptions<PrimeDbContext> options)
            : base(options)
        {
        }

        // from stored procedures
        public virtual DbSet<GuestArrival> GuestArrivals { get; set; }
    }
    /*//  usage:
    [HttpGet("GetGuestsForDate")]
    public IActionResult GetData([FromQuery] string date) date="21-03-2021"
    {
        var guests = _context.GuestArrival.FromSqlInterpolated($"GetGuestsForDate '{date}'").ToList();
        return Ok(guests);
    }*/

}
