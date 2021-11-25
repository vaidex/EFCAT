using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    public partial class tmtos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TEST_MTOS_TEST_ENTITIES_TEST_ID_TEST_NAME",
                table: "TEST_MTOS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TEST_MTOS",
                table: "TEST_MTOS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TEST_ENTITIES",
                table: "TEST_ENTITIES");

            migrationBuilder.DropColumn(
                name: "TEST_NAME",
                table: "TEST_MTOS");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TEST_MTOS",
                table: "TEST_MTOS",
                column: "TEST_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TEST_ENTITIES",
                table: "TEST_ENTITIES",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_TEST_MTOS_TEST_ENTITIES_TEST_ID",
                table: "TEST_MTOS",
                column: "TEST_ID",
                principalTable: "TEST_ENTITIES",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TEST_MTOS_TEST_ENTITIES_TEST_ID",
                table: "TEST_MTOS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TEST_MTOS",
                table: "TEST_MTOS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TEST_ENTITIES",
                table: "TEST_ENTITIES");

            migrationBuilder.AddColumn<string>(
                name: "TEST_NAME",
                table: "TEST_MTOS",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TEST_MTOS",
                table: "TEST_MTOS",
                columns: new[] { "TEST_ID", "TEST_NAME" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TEST_ENTITIES",
                table: "TEST_ENTITIES",
                columns: new[] { "ID", "NAME" });

            migrationBuilder.AddForeignKey(
                name: "FK_TEST_MTOS_TEST_ENTITIES_TEST_ID_TEST_NAME",
                table: "TEST_MTOS",
                columns: new[] { "TEST_ID", "TEST_NAME" },
                principalTable: "TEST_ENTITIES",
                principalColumns: new[] { "ID", "NAME" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
