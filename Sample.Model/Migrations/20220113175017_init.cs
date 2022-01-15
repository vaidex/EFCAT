using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.Model.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TEST_ENTITIES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NAME = table.Column<string>(type: "varchar(16)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PASSWORD = table.Column<string>(type: "varchar(256)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NUMBER = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    TYPE = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEST_ENTITIES", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TEST_MTOS",
                columns: table => new
                {
                    TEST_ID = table.Column<int>(type: "int", nullable: false),
                    TEST_SECOND_ID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEST_MTOS", x => x.TEST_ID);
                    table.ForeignKey(
                        name: "FK_TEST_MTOS_TEST_ENTITIES_TEST_ID",
                        column: x => x.TEST_ID,
                        principalTable: "TEST_ENTITIES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TEST_MTOS_TEST_ENTITIES_TEST_SECOND_ID",
                        column: x => x.TEST_SECOND_ID,
                        principalTable: "TEST_ENTITIES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TEST_ENTITIES_NAME",
                table: "TEST_ENTITIES",
                column: "NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TEST_MTOS_TEST_SECOND_ID",
                table: "TEST_MTOS",
                column: "TEST_SECOND_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TEST_MTOS");

            migrationBuilder.DropTable(
                name: "TEST_ENTITIES");
        }
    }
}
