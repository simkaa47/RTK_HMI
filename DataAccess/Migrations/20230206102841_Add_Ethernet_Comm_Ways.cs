using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class Add_Ethernet_Comm_Ways : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "Ip",
                table: "ConnectSettingses",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<int>(
                name: "PortNumber",
                table: "ConnectSettingses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Way",
                table: "ConnectSettingses",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ip",
                table: "ConnectSettingses");

            migrationBuilder.DropColumn(
                name: "PortNumber",
                table: "ConnectSettingses");

            migrationBuilder.DropColumn(
                name: "Way",
                table: "ConnectSettingses");
        }
    }
}
