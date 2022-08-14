using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Migrations
{
    public partial class UpdateCheckoutAndBookModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checkouts_AspNetUsers_LibraryUserId1",
                table: "Checkouts");

            migrationBuilder.DropColumn(
                name: "LibraryUserId",
                table: "Checkouts");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "LibraryUserId1",
                table: "Checkouts",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Checkouts_LibraryUserId1",
                table: "Checkouts",
                newName: "IX_Checkouts_UserId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Books",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Checkouts_AspNetUsers_UserId",
                table: "Checkouts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checkouts_AspNetUsers_UserId",
                table: "Checkouts");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Checkouts",
                newName: "LibraryUserId1");

            migrationBuilder.RenameIndex(
                name: "IX_Checkouts_UserId",
                table: "Checkouts",
                newName: "IX_Checkouts_LibraryUserId1");

            migrationBuilder.AddColumn<int>(
                name: "LibraryUserId",
                table: "Checkouts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "Picture",
                table: "Books",
                type: "longblob",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Checkouts_AspNetUsers_LibraryUserId1",
                table: "Checkouts",
                column: "LibraryUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
