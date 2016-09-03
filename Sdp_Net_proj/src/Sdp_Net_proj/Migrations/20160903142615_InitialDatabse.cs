using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sdp_Net_proj.Migrations
{
    public partial class InitialDatabse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Passowrd = table.Column<string>(nullable: true),
                    User_Name = table.Column<string>(nullable: true),
                    User_score = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    UserID = table.Column<string>(nullable: true),
                    player1 = table.Column<string>(nullable: true),
                    player1_score = table.Column<int>(nullable: false),
                    player2 = table.Column<string>(nullable: true),
                    player2_score = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Games_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Step",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    GameID = table.Column<string>(nullable: true),
                    Letters_Indexs = table.Column<string>(nullable: true),
                    Player1_Score = table.Column<int>(nullable: false),
                    Player2_Score = table.Column<int>(nullable: false),
                    Step_Number = table.Column<int>(nullable: false),
                    Word = table.Column<string>(nullable: true),
                    Word_Score = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Step", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Step_Games_GameID",
                        column: x => x.GameID,
                        principalTable: "Games",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_UserID",
                table: "Games",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Step_GameID",
                table: "Step",
                column: "GameID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Step");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
