using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobOffersRestApi.Migrations
{
    public partial class AddCvFileNameToJobApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalInfo",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CvFileName",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalInfo",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "CvFileName",
                table: "Applications");
        }
    }
}
