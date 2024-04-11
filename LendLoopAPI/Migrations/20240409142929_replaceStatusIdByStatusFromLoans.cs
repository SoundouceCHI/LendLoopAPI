using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LendLoopAPI.Migrations
{
    /// <inheritdoc />
    public partial class replaceStatusIdByStatusFromLoans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Loans");

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "Loans",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "Loans");

            migrationBuilder.AddColumn<string>(
                name: "StatusId",
                table: "Loans",
                type: "int", 
                nullable: false,
                defaultValue: "");
        }
    }
}
