using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLeagueEntries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeagueEntry_Leagues_LeagueId",
                table: "LeagueEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_LeagueEntry_Teams_TeamId",
                table: "LeagueEntry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LeagueEntry",
                table: "LeagueEntry");

            migrationBuilder.RenameTable(
                name: "LeagueEntry",
                newName: "LeagueEntries");

            migrationBuilder.RenameIndex(
                name: "IX_LeagueEntry_TeamId",
                table: "LeagueEntries",
                newName: "IX_LeagueEntries_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_LeagueEntry_LeagueId",
                table: "LeagueEntries",
                newName: "IX_LeagueEntries_LeagueId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "JoinedAt",
                table: "LeagueEntries",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "SYSUTCDATETIME()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<Guid>(
                name: "LeagueEntryId",
                table: "LeagueEntries",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeagueEntries",
                table: "LeagueEntries",
                column: "LeagueEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_LeagueEntries_LeagueId_TeamId",
                table: "LeagueEntries",
                columns: new[] { "LeagueId", "TeamId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LeagueEntries_Leagues_LeagueId",
                table: "LeagueEntries",
                column: "LeagueId",
                principalTable: "Leagues",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeagueEntries_Teams_TeamId",
                table: "LeagueEntries",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeagueEntries_Leagues_LeagueId",
                table: "LeagueEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_LeagueEntries_Teams_TeamId",
                table: "LeagueEntries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LeagueEntries",
                table: "LeagueEntries");

            migrationBuilder.DropIndex(
                name: "IX_LeagueEntries_LeagueId_TeamId",
                table: "LeagueEntries");

            migrationBuilder.RenameTable(
                name: "LeagueEntries",
                newName: "LeagueEntry");

            migrationBuilder.RenameIndex(
                name: "IX_LeagueEntries_TeamId",
                table: "LeagueEntry",
                newName: "IX_LeagueEntry_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_LeagueEntries_LeagueId",
                table: "LeagueEntry",
                newName: "IX_LeagueEntry_LeagueId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "JoinedAt",
                table: "LeagueEntry",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "SYSUTCDATETIME()");

            migrationBuilder.AlterColumn<Guid>(
                name: "LeagueEntryId",
                table: "LeagueEntry",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "NEWID()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeagueEntry",
                table: "LeagueEntry",
                column: "LeagueEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeagueEntry_Leagues_LeagueId",
                table: "LeagueEntry",
                column: "LeagueId",
                principalTable: "Leagues",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeagueEntry_Teams_TeamId",
                table: "LeagueEntry",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
