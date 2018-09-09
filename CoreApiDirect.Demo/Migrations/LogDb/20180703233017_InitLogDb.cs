using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreApiDirect.Demo.Migrations.LogDb
{
    public partial class InitLogDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogEvents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogDetail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LogDate = table.Column<DateTime>(nullable: false),
                    Text = table.Column<string>(maxLength: 255, nullable: false),
                    LogEventId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogDetail_LogEvents_LogEventId",
                        column: x => x.LogEventId,
                        principalTable: "LogEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SystemInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OS = table.Column<string>(maxLength: 25, nullable: false),
                    OSVersion = table.Column<string>(maxLength: 10, nullable: false),
                    LogEventId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemInfo_LogEvents_LogEventId",
                        column: x => x.LogEventId,
                        principalTable: "LogEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogDetail_LogEventId",
                table: "LogDetail",
                column: "LogEventId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SystemInfo_LogEventId",
                table: "SystemInfo",
                column: "LogEventId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogDetail");

            migrationBuilder.DropTable(
                name: "SystemInfo");

            migrationBuilder.DropTable(
                name: "LogEvents");
        }
    }
}
