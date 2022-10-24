using Microsoft.EntityFrameworkCore.Migrations;

namespace GuitarTunings.Migrations
{
    public partial class AlbumId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlbumArtists_Albums_AlbumArtistId1",
                table: "AlbumArtists");

            migrationBuilder.DropIndex(
                name: "IX_AlbumArtists_AlbumArtistId1",
                table: "AlbumArtists");

            migrationBuilder.DropColumn(
                name: "AlbumArtistId1",
                table: "AlbumArtists");

            migrationBuilder.RenameColumn(
                name: "ArtistId",
                table: "Albums",
                newName: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumArtists_AlbumId",
                table: "AlbumArtists",
                column: "AlbumId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumArtists_Albums_AlbumId",
                table: "AlbumArtists",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "AlbumId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlbumArtists_Albums_AlbumId",
                table: "AlbumArtists");

            migrationBuilder.DropIndex(
                name: "IX_AlbumArtists_AlbumId",
                table: "AlbumArtists");

            migrationBuilder.RenameColumn(
                name: "AlbumId",
                table: "Albums",
                newName: "ArtistId");

            migrationBuilder.AddColumn<int>(
                name: "AlbumArtistId1",
                table: "AlbumArtists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AlbumArtists_AlbumArtistId1",
                table: "AlbumArtists",
                column: "AlbumArtistId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumArtists_Albums_AlbumArtistId1",
                table: "AlbumArtists",
                column: "AlbumArtistId1",
                principalTable: "Albums",
                principalColumn: "ArtistId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
