using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cShipment.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDriverShipmentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // --- START: Changes for DriverShipments table (These are correct and needed) ---
            migrationBuilder.DropForeignKey(
                name: "FK_DriverShipments_Drivers_DriverId",
                table: "DriverShipments");

            migrationBuilder.DropForeignKey(
                name: "FK_DriverShipments_Shipments_ShipmentId",
                table: "DriverShipments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DriverShipments",
                table: "DriverShipments");

            migrationBuilder.AddColumn<int>(
                name: "DriverShipmentId",
                table: "DriverShipments",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1"); // Adds the new primary key column

            migrationBuilder.AddPrimaryKey(
                name: "PK_DriverShipments",
                table: "DriverShipments",
                column: "DriverShipmentId"); // Sets the new primary key

            migrationBuilder.CreateIndex(
                name: "IX_DriverShipments_DriverId",
                table: "DriverShipments",
                column: "DriverId"); // Re-creates index for DriverId if needed

            // Re-add foreign keys with the desired delete behavior (Restrict or Cascade)
            // Note: Your original migration used Restrict, which is generally safer to prevent accidental data loss.
            migrationBuilder.AddForeignKey(
                name: "FK_DriverShipments_Drivers_DriverId",
                table: "DriverShipments",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "DriverId",
                onDelete: ReferentialAction.Restrict); // Or .Cascade if you prefer drivers/shipments to be deleted when their FK is deleted

            migrationBuilder.AddForeignKey(
                name: "FK_DriverShipments_Shipments_ShipmentId",
                table: "DriverShipments",
                column: "ShipmentId",
                principalTable: "Shipments",
                principalColumn: "ShipmentId",
                onDelete: ReferentialAction.Restrict); // Or .Cascade
            // --- END: Changes for DriverShipments table ---


            // --- START: Problematic changes for ASP.NET Core Identity tables (COMMENTED OUT) ---
            // These lines caused the "The object 'PK_AspNetUserTokens' is dependent on column 'Name'" error.
            // They are attempting to alter columns that are part of composite primary keys without
            // properly dropping and re-creating the primary keys first, which SQL Server disallows directly.
            // It's best to address any intended changes to Identity tables separately and carefully,
            // or ensure your Identity models match EF Core's conventions.

            // migrationBuilder.AlterColumn<string>(
            //     name: "Name",
            //     table: "AspNetUserTokens",
            //     type: "nvarchar(128)",
            //     maxLength: 128,
            //     nullable: false,
            //     oldClrType: typeof(string),
            //     oldType: "nvarchar(450)");

            // migrationBuilder.AlterColumn<string>(
            //     name: "LoginProvider",
            //     table: "AspNetUserTokens",
            //     type: "nvarchar(128)",
            //     maxLength: 128,
            //     nullable: false,
            //     oldClrType: typeof(string),
            //     oldType: "nvarchar(450)");

            // migrationBuilder.AlterColumn<string>(
            //     name: "ProviderKey",
            //     table: "AspNetUserLogins",
            //     type: "nvarchar(128)",
            //     maxLength: 128,
            //     nullable: false,
            //     oldClrType: typeof(string),
            //     oldType: "nvarchar(450)");

            // migrationBuilder.AlterColumn<string>(
            //     name: "LoginProvider",
            //     table: "AspNetUserLogins",
            //     type: "nvarchar(128)",
            //     maxLength: 128,
            //     nullable: false,
            //     oldClrType: typeof(string),
            //     oldType: "nvarchar(450)");
            // --- END: Problematic changes (COMMENTED OUT) ---
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // --- START: Revert changes for DriverShipments table (These are correct and needed) ---
            migrationBuilder.DropForeignKey(
                name: "FK_DriverShipments_Drivers_DriverId",
                table: "DriverShipments");

            migrationBuilder.DropForeignKey(
                name: "FK_DriverShipments_Shipments_ShipmentId",
                table: "DriverShipments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DriverShipments",
                table: "DriverShipments");

            migrationBuilder.DropIndex(
                name: "IX_DriverShipments_DriverId",
                table: "DriverShipments");

            migrationBuilder.DropColumn(
                name: "DriverShipmentId",
                table: "DriverShipments");

            // Re-create the old composite primary key (DriverId, ShipmentId)
            migrationBuilder.AddPrimaryKey(
                name: "PK_DriverShipments",
                table: "DriverShipments",
                columns: new[] { "DriverId", "ShipmentId" }); // Re-creates the composite primary key

            // Re-add foreign keys with original cascade behavior
            migrationBuilder.AddForeignKey(
                name: "FK_DriverShipments_Drivers_DriverId",
                table: "DriverShipments",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "DriverId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DriverShipments_Shipments_ShipmentId",
                table: "DriverShipments",
                column: "ShipmentId",
                principalTable: "Shipments",
                principalColumn: "ShipmentId",
                onDelete: ReferentialAction.Cascade);
            // --- END: Revert changes for DriverShipments table ---


            // --- START: Revert problematic changes for ASP.NET Core Identity tables (COMMENTED OUT) ---
            // These lines correspond to the commented-out changes in the Up method.

            // migrationBuilder.AlterColumn<string>(
            //     name: "Name",
            //     table: "AspNetUserTokens",
            //     type: "nvarchar(450)",
            //     nullable: false,
            //     oldClrType: typeof(string),
            //     oldType: "nvarchar(128)",
            //     oldMaxLength: 128);

            // migrationBuilder.AlterColumn<string>(
            //     name: "LoginProvider",
            //     table: "AspNetUserTokens",
            //     type: "nvarchar(450)",
            //     nullable: false,
            //     oldClrType: typeof(string),
            //     oldType: "nvarchar(128)",
            //     oldMaxLength: 128);

            // migrationBuilder.AlterColumn<string>(
            //     name: "ProviderKey",
            //     table: "AspNetUserLogins",
            //     type: "nvarchar(450)",
            //     nullable: false,
            //     oldClrType: typeof(string),
            //     oldType: "nvarchar(128)",
            //     oldMaxLength: 128);

            // migrationBuilder.AlterColumn<string>(
            //     name: "LoginProvider",
            //     table: "AspNetUserLogins",
            //     type: "nvarchar(450)",
            //     nullable: false,
            //     oldClrType: typeof(string),
            //     oldType: "nvarchar(128)",
            //     oldMaxLength: 128);
            // --- END: Revert problematic changes (COMMENTED OUT) ---
        }
    }
}