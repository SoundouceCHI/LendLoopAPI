using LendLoopAPI.Models;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;


#nullable disable

namespace LendLoopAPI.Migrations
{
    /// <inheritdoc />
    public partial class addSubcategoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subcategories", 
                columns:  table => new
                {
                    SubcategoryId = table.Column<int>(nullable: false)
                                    .Annotation("SqlServer:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SubcategoryName = table.Column<string>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false)
                }, 
                constraints: table =>
                {
                    table.PrimaryKey("Pk_Subcategories", x => x.SubcategoryId);
                    table.ForeignKey("Fk_Subcategories", x => x.CategoryId, principalTable: "Categories", 
                        principalColumn: "CategoryId", onDelete: ReferentialAction.Cascade); 
                }

                ); 
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subcategories"); 
        }
    }
}
