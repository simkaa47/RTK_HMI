using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class Add_Y_To_Calibration_point : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParameterIdY",
                table: "CalibrationCells",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "UseCastomValueY",
                table: "CalibrationCells",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<float>(
                name: "ValueY",
                table: "CalibrationCells",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParameterIdY",
                table: "CalibrationCells");

            migrationBuilder.DropColumn(
                name: "UseCastomValueY",
                table: "CalibrationCells");

            migrationBuilder.DropColumn(
                name: "ValueY",
                table: "CalibrationCells");
        }
    }
}
