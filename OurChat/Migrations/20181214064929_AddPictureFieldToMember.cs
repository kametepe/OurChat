using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OurChat.Migrations
{
    public partial class AddPictureFieldToMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PicType",
                table: "Members",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Picture",
                table: "Members",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PicType",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Members");
        }
    }
}
