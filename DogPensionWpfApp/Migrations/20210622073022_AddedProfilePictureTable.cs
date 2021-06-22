using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DogPensionWpfApp.Migrations
{
    public partial class AddedProfilePictureTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfilePictureId",
                table: "Owners",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfilePictureId",
                table: "Dogs",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProfilePictures",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(nullable: false),
                    FileType = table.Column<string>(nullable: false),
                    Picture = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfilePictures", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Owners_ProfilePictureId",
                table: "Owners",
                column: "ProfilePictureId");

            migrationBuilder.CreateIndex(
                name: "IX_Dogs_ProfilePictureId",
                table: "Dogs",
                column: "ProfilePictureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dogs_ProfilePictures_ProfilePictureId",
                table: "Dogs",
                column: "ProfilePictureId",
                principalTable: "ProfilePictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Owners_ProfilePictures_ProfilePictureId",
                table: "Owners",
                column: "ProfilePictureId",
                principalTable: "ProfilePictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dogs_ProfilePictures_ProfilePictureId",
                table: "Dogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Owners_ProfilePictures_ProfilePictureId",
                table: "Owners");

            migrationBuilder.DropTable(
                name: "ProfilePictures");

            migrationBuilder.DropIndex(
                name: "IX_Owners_ProfilePictureId",
                table: "Owners");

            migrationBuilder.DropIndex(
                name: "IX_Dogs_ProfilePictureId",
                table: "Dogs");

            migrationBuilder.DropColumn(
                name: "ProfilePictureId",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "ProfilePictureId",
                table: "Dogs");
        }
    }
}
