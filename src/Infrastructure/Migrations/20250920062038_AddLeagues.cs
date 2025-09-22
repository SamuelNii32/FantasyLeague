using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLeagues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_League_Users_CreatedByUserId",
                table: "League");

            migrationBuilder.DropForeignKey(
                name: "FK_LeagueEntry_League_LeagueId",
                table: "LeagueEntry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_League",
                table: "League");

            migrationBuilder.DropIndex(
                name: "IX_League_CreatedByUserId",
                table: "League");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "League");

            migrationBuilder.RenameTable(
                name: "League",
                newName: "Leagues");

            migrationBuilder.AlterColumn<byte>(
                name: "Type",
                table: "Leagues",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<string>(
                name: "LeagueName",
                table: "Leagues",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Leagues",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "SYSUTCDATETIME()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<Guid>(
                name: "LeagueId",
                table: "Leagues",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Leagues",
                table: "Leagues",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_Leagues_CreatedBy",
                table: "Leagues",
                column: "CreatedBy");

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

            migrationBuilder.AddForeignKey(
                name: "FK_LeagueEntry_Leagues_LeagueId",
                table: "LeagueEntry",
                column: "LeagueId",
                principalTable: "Leagues",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Leagues_Users_CreatedBy",
                table: "Leagues",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeagueEntry_Leagues_LeagueId",
                table: "LeagueEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_Leagues_Users_CreatedBy",
                table: "Leagues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Leagues",
                table: "Leagues");

            migrationBuilder.DropIndex(
                name: "IX_Leagues_CreatedBy",
                table: "Leagues");

            migrationBuilder.DropIndex(
                name: "IX_Leagues_Type_StartDate",
                table: "Leagues");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Leagues_Dates_Order",
                table: "Leagues");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Leagues_MaxTeams_NonNeg",
                table: "Leagues");

            migrationBuilder.RenameTable(
                name: "Leagues",
                newName: "League");

            migrationBuilder.AlterColumn<byte>(
                name: "Type",
                table: "League",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldDefaultValue: (byte)0);

            migrationBuilder.AlterColumn<string>(
                name: "LeagueName",
                table: "League",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "League",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "SYSUTCDATETIME()");

            migrationBuilder.AlterColumn<Guid>(
                name: "LeagueId",
                table: "League",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "NEWID()");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "League",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_League",
                table: "League",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_League_CreatedByUserId",
                table: "League",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_League_Users_CreatedByUserId",
                table: "League",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LeagueEntry_League_LeagueId",
                table: "LeagueEntry",
                column: "LeagueId",
                principalTable: "League",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
