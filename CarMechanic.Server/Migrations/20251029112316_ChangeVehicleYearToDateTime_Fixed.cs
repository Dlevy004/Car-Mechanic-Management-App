using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarMechanic.Server.Migrations
{
    /// <inheritdoc />
    public partial class ChangeVehicleYearToDateTime_Fixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ""Jobs"" ALTER COLUMN ""VehicleYear"" TYPE timestamp with time zone
                USING make_date(""VehicleYear"", 1, 1);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ""Jobs"" ALTER COLUMN ""VehicleYear"" TYPE integer
                USING EXTRACT(YEAR FROM ""VehicleYear"");
            ");
        }
    }
}
