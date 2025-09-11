using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YASAM.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddsAutomaticIdlingConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutomaticIdlingConfigurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AppId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    GameName = table.Column<string>(type: "TEXT", nullable: false),
                    IdleTime = table.Column<int>(type: "INTEGER", nullable: false),
                    CronTickerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutomaticIdlingConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AutomaticIdlingConfigurations_CronTickers_CronTickerId",
                        column: x => x.CronTickerId,
                        principalSchema: "ticker",
                        principalTable: "CronTickers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutomaticIdlingConfigurations_CronTickerId",
                table: "AutomaticIdlingConfigurations",
                column: "CronTickerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutomaticIdlingConfigurations");
        }
    }
}
