using Microsoft.EntityFrameworkCore.Migrations;

namespace GuitarTunings.Migrations
{
    public partial class TuningSongOneToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Tunings",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "TuningId",
                table: "Songs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Songs_TuningId",
                table: "Songs",
                column: "TuningId");

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Tunings_TuningId",
                table: "Songs",
                column: "TuningId",
                principalTable: "Tunings",
                principalColumn: "TuningId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Tunings_TuningId",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_Songs_TuningId",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "TuningId",
                table: "Songs");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Tunings",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);
        }
    }
}
