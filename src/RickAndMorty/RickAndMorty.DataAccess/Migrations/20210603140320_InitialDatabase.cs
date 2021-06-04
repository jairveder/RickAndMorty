using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RickAndMorty.DataAccess.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Episode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FullName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Url = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Origin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Url = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Origin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Character",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Status = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Species = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "longtext", nullable: true),
                    Gender = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    OriginId = table.Column<int>(type: "int", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    Image = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Url = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime(6)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Character", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Character_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Character_Origin_OriginId",
                        column: x => x.OriginId,
                        principalTable: "Origin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CharacterEpisode",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    EpisodeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterEpisode", x => new { x.CharacterId, x.EpisodeId });
                    table.ForeignKey(
                        name: "FK_CharacterEpisode_Character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterEpisode_Episode_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "Episode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Character_LocationId",
                table: "Character",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Character_Name",
                table: "Character",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Character_OriginId",
                table: "Character",
                column: "OriginId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterEpisode_EpisodeId",
                table: "CharacterEpisode",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_Name",
                table: "Location",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterEpisode");

            migrationBuilder.DropTable(
                name: "Character");

            migrationBuilder.DropTable(
                name: "Episode");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "Origin");
        }
    }
}
