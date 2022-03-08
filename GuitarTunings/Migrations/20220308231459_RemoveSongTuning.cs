using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GuitarTunings.Migrations
{
    public partial class RemoveSongTuning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SongTunings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SongTunings",
                columns: table => new
                {
                    SongTuningId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SongId = table.Column<int>(type: "int", nullable: false),
                    TuningId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SongTunings", x => x.SongTuningId);
                    table.ForeignKey(
                        name: "FK_SongTunings_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "SongId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SongTunings_Tunings_TuningId",
                        column: x => x.TuningId,
                        principalTable: "Tunings",
                        principalColumn: "TuningId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SongTunings_SongId",
                table: "SongTunings",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_SongTunings_TuningId",
                table: "SongTunings",
                column: "TuningId");
        }
    }
}
