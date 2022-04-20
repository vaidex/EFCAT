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
                name: "BAD_PEOPLE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PERSON_FIRST_NAME = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PERSON_LAST_NAME = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PERSON_GENDER = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BAD_PEOPLE", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FOREIGN_MULTI_KEYS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    NAME = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DATE = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FOREIGN_MULTI_KEYS", x => new { x.ID, x.NAME, x.DATE });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NICE_PEOPLE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FIRST_NAME = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LAST_NAME = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GENDER = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NICE_PEOPLE", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NOT_GENERATED_KEY_IMPLEMENT",
                columns: table => new
                {
                    SECOND_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NOT_GENERATED_KEY_ID = table.Column<int>(type: "int", nullable: false),
                    NOT_GENERATED_KEY_NAME = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NOT_GENERATED_KEY_IMPLEMENT", x => x.SECOND_ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NOT_GENERATED_KEY_INHERIT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SECOND_ID = table.Column<int>(type: "int", nullable: false),
                    NAME = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NOT_GENERATED_KEY_INHERIT", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NOT_GENERATED_KEY_INHERIT_2ND",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SECOND_ID = table.Column<int>(type: "int", nullable: false),
                    NAME = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NOT_GENERATED_KEY_INHERIT_2ND", x => x.ID);
                })
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
                    BALANCE = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    IMAGE_CONTENT = table.Column<byte[]>(type: "longblob", nullable: true),
                    IMAGE_TYPE = table.Column<string>(type: "varchar(32)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IMPL_TEXT = table.Column<string>(type: "varchar(32)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FOREIGN_MULTI_KEYS_CUSTOMIZED",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    NAME = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DATE = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FOREIGN_MULTI_KEYS_CUSTOMIZED", x => new { x.ID, x.NAME });
                    table.ForeignKey(
                        name: "FK_FOREIGN_MULTI_KEYS_CUSTOMIZED_FOREIGN_MULTI_KEYS_ID_NAME_DATE",
                        columns: x => new { x.ID, x.NAME, x.DATE },
                        principalTable: "FOREIGN_MULTI_KEYS",
                        principalColumns: new[] { "ID", "NAME", "DATE" },
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FOREIGN_MULTI_KEYS_DEFAULT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    NAME = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DATE = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FOREIGN_MULTI_KEYS_DEFAULT", x => new { x.ID, x.NAME, x.DATE });
                    table.ForeignKey(
                        name: "FK_FOREIGN_MULTI_KEYS_DEFAULT_FOREIGN_MULTI_KEYS_ID_NAME_DATE",
                        columns: x => new { x.ID, x.NAME, x.DATE },
                        principalTable: "FOREIGN_MULTI_KEYS",
                        principalColumns: new[] { "ID", "NAME", "DATE" },
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ROLES",
                columns: table => new
                {
                    ROLE_NAME = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    USER_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLES", x => new { x.USER_ID, x.ROLE_NAME });
                    table.ForeignKey(
                        name: "FK_ROLES_USERS_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "USERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "USER_HAS_CODES",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    USER_ID = table.Column<int>(type: "int", nullable: true),
                    VALUE = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EXPIRES_AT = table.Column<DateTime>(type: "datetime(6)", nullable: false),
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
                name: "IX_FOREIGN_MULTI_KEYS_CUSTOMIZED_ID_NAME_DATE",
                table: "FOREIGN_MULTI_KEYS_CUSTOMIZED",
                columns: new[] { "ID", "NAME", "DATE" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USER_HAS_CODES_USER_ID",
                table: "USER_HAS_CODES",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "UQ_USER_EMAIL",
                table: "USERS",
                column: "EMAIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_USER_NAME",
                table: "USERS",
                column: "NAME",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ADVANCED_EMAIL_CODES");

            migrationBuilder.DropTable(
                name: "BAD_PEOPLE");

            migrationBuilder.DropTable(
                name: "FOREIGN_MULTI_KEYS_CUSTOMIZED");

            migrationBuilder.DropTable(
                name: "FOREIGN_MULTI_KEYS_DEFAULT");

            migrationBuilder.DropTable(
                name: "NICE_PEOPLE");

            migrationBuilder.DropTable(
                name: "NOT_GENERATED_KEY_IMPLEMENT");

            migrationBuilder.DropTable(
                name: "NOT_GENERATED_KEY_INHERIT");

            migrationBuilder.DropTable(
                name: "NOT_GENERATED_KEY_INHERIT_2ND");

            migrationBuilder.DropTable(
                name: "ROLES");

            migrationBuilder.DropTable(
                name: "ZMAILS");

            migrationBuilder.DropTable(
                name: "USER_HAS_CODES");

            migrationBuilder.DropTable(
                name: "FOREIGN_MULTI_KEYS");

            migrationBuilder.DropTable(
                name: "USERS");
        }
    }
}
