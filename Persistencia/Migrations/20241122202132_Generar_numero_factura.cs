using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistencia.Migrations
{
    public partial class Generar_numero_factura : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "numero_factura",
                table: "factura",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_factura_numero_factura",
                table: "factura",
                column: "numero_factura",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_clientes_numero_identificacion",
                table: "clientes",
                column: "numero_identificacion",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_factura_numero_factura",
                table: "factura");

            migrationBuilder.DropIndex(
                name: "IX_clientes_numero_identificacion",
                table: "clientes");

            migrationBuilder.DropColumn(
                name: "numero_factura",
                table: "factura");
        }
    }
}
