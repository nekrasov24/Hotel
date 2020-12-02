using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RoomService.Migrations
{
    public partial class AddRoomToRoomService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Number = table.Column<int>(nullable: false),
                    NumberOfPeople = table.Column<int>(nullable: false),
                    PriceForNight = table.Column<decimal>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    RoomType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
