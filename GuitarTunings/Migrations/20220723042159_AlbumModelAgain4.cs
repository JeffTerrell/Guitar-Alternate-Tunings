using Microsoft.EntityFrameworkCore.Migrations;

namespace GuitarTunings.Migrations
{
    public partial class AlbumModelAgain4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumArtists_Albums_AlbumId",
                table: "AlbumArtists",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "AlbumId",
                onDelete: ReferentialAction.Cascade);
            

            migrationBuilder.DropColumn(
                name: "AlbumArtistId1",
                table: "AlbumArtists");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}