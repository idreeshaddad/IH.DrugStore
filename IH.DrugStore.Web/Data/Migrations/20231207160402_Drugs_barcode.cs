using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IH.DrugStore.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class Drugs_barcode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BarCode",
                table: "Drugs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BarCode",
                table: "Drugs");
        }
    }
}
