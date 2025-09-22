using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLeagues_AddJoinCode_StartGW : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Leagues_Type_StartDate",
                table: "Leagues");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Leagues_Dates_Order",
                table: "Leagues");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Leagues_MaxTeams_NonNeg",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Leagues");

            migrationBuilder.AlterColumn<string>(
                name: "LeagueName",
                table: "Leagues",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "JoinCode",
                table: "Leagues",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StartGameweekId",
                table: "Leagues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Leagues_JoinCode",
                table: "Leagues",
                column: "JoinCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Leagues_Type_StartGameweekId",
                table: "Leagues",
                columns: new[] { "Type", "StartGameweekId" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Leagues_JoinCode_Length",
                table: "Leagues",
                sql: "LEN([JoinCode]) BETWEEN 8 AND 12");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Leagues_Type_MaxTeams",
                table: "Leagues",
                sql: "([Type] = 0 AND [MaxTeams] IS NULL) OR ([Type] = 1 AND [MaxTeams] IS NOT NULL AND [MaxTeams] BETWEEN 4 AND 20 AND ([MaxTeams] % 2) = 0)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Leagues_JoinCode",
                table: "Leagues");

            migrationBuilder.DropIndex(
                name: "IX_Leagues_Type_StartGameweekId",
                table: "Leagues");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Leagues_JoinCode_Length",
                table: "Leagues");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Leagues_Type_MaxTeams",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "JoinCode",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "StartGameweekId",
                table: "Leagues");

            migrationBuilder.AlterColumn<string>(
                name: "LeagueName",
                table: "Leagues",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Leagues",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Leagues",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Leagues_Type_StartDate",
                table: "Leagues",
                columns: new[] { "Type", "StartDate" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Leagues_Dates_Order",
                table: "Leagues",
                sql: "[StartDate] IS NULL OR [EndDate] IS NULL OR [StartDate] <= [EndDate]");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Leagues_MaxTeams_NonNeg",
                table: "Leagues",
                sql: "[MaxTeams] IS NULL OR [MaxTeams] >= 2");
        }
    }
}
