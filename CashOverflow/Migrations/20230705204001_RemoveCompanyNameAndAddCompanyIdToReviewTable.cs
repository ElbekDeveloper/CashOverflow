using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashOverflow.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCompanyNameAndAddCompanyIdToReviewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Reviews");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "Reviews",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Reviews");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
