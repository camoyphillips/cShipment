using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cShipment.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    DriverId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LicenseNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.DriverId);
                });

            migrationBuilder.CreateTable(
                name: "Trucks",
                columns: table => new
                {
                    TruckId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Mileage = table.Column<double>(type: "float", nullable: false),
                    LastMaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssignedDriverId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trucks", x => x.TruckId);
                    table.ForeignKey(
                        name: "FK_Trucks_Drivers_AssignedDriverId",
                        column: x => x.AssignedDriverId,
                        principalTable: "Drivers",
                        principalColumn: "DriverId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Shipments",
                columns: table => new
                {
                    ShipmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Origin = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Distance = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TruckId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.ShipmentId);
                    table.ForeignKey(
                        name: "FK_Shipments_Trucks_TruckId",
                        column: x => x.TruckId,
                        principalTable: "Trucks",
                        principalColumn: "TruckId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DriverShipments",
                columns: table => new
                {
                    DriverId = table.Column<int>(type: "int", nullable: false),
                    ShipmentId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AssignedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverShipments", x => new { x.DriverId, x.ShipmentId });
                    table.ForeignKey(
                        name: "FK_DriverShipments_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "DriverId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DriverShipments_Shipments_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "Shipments",
                        principalColumn: "ShipmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DriverShipments_ShipmentId",
                table: "DriverShipments",
                column: "ShipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_TruckId",
                table: "Shipments",
                column: "TruckId");

            migrationBuilder.CreateIndex(
                name: "IX_Trucks_AssignedDriverId",
                table: "Trucks",
                column: "AssignedDriverId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverShipments");

            migrationBuilder.DropTable(
                name: "Shipments");

            migrationBuilder.DropTable(
                name: "Trucks");

            migrationBuilder.DropTable(
                name: "Drivers");
        }
    }
}
