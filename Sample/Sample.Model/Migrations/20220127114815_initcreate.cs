using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.Model.Migrations
{
    public partial class initcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EMAIL = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NAME = table.Column<string>(type: "varchar(16)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PASSWORD = table.Column<string>(type: "varchar(256)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BALANCE = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    IMAGE_CONTENT = table.Column<byte[]>(type: "longblob", nullable: true),
                    IMAGE_TYPE = table.Column<string>(type: "varchar(32)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "USER_HAS_CODES",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    USER_ID = table.Column<int>(type: "int", nullable: true),
                    TYPE = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VALUE = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EXPIRES_AT = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    QR_CONTENT = table.Column<byte[]>(type: "longblob", nullable: false),
                    QR_TYPE = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DISCRIMINATOR = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_HAS_CODES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_USER_HAS_CODES_USERS_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "USERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ZMAILS",
                columns: table => new
                {
                    USER_ID = table.Column<int>(type: "int", nullable: false),
                    VALUE = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZMAILS", x => x.USER_ID);
                    table.ForeignKey(
                        name: "FK_ZMAILS_USERS_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "USERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ADVANCED_EMAIL_CODES",
                columns: table => new
                {
                    RANDOM = table.Column<int>(type: "int(5)", nullable: false),
                    CODE_ID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IMAGE_CONTENT = table.Column<byte[]>(type: "longblob", nullable: false),
                    IMAGE_TYPE = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADVANCED_EMAIL_CODES", x => new { x.CODE_ID, x.RANDOM });
                    table.ForeignKey(
                        name: "FK_ADVANCED_EMAIL_CODES_USER_HAS_CODES_CODE_ID",
                        column: x => x.CODE_ID,
                        principalTable: "USER_HAS_CODES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_USER_HAS_CODES_USER_ID",
                table: "USER_HAS_CODES",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_USERS_EMAIL",
                table: "USERS",
                column: "EMAIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USERS_NAME",
                table: "USERS",
                column: "NAME",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ADVANCED_EMAIL_CODES");

            migrationBuilder.DropTable(
                name: "ZMAILS");

            migrationBuilder.DropTable(
                name: "USER_HAS_CODES");

            migrationBuilder.DropTable(
                name: "USERS");
        }
    }
}
