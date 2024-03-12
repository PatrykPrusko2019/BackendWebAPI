using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddTableItemsOfProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemsOfProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameProduct = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodeProduct = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemsOfProducts = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemsOfProduct", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemsOfProduct");
        }
    }
}
