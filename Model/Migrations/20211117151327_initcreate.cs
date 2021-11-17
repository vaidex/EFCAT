using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    public partial class initcreate : Migration
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
                    NAME = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PASSWORD = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NUMBER = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEST_ENTITIES", x => new { x.ID, x.NAME });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TEST_MTOS",
                columns: table => new
                {
                    TEST_ID = table.Column<int>(type: "int", nullable: false),
                    TEST_NAME = table.Column<string>(type: "varchar(20)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEST_MTOS", x => new { x.TEST_ID, x.TEST_NAME });
                    table.ForeignKey(
                        name: "FK_TEST_MTOS_TEST_ENTITIES_TEST_ID_TEST_NAME",
                        columns: x => new { x.TEST_ID, x.TEST_NAME },
                        principalTable: "TEST_ENTITIES",
                        principalColumns: new[] { "ID", "NAME" },
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TEST_ENTITIES_NAME",
                table: "TEST_ENTITIES",
                column: "NAME",
                unique: true);
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
