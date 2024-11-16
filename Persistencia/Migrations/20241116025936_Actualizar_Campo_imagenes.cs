using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistencia.Migrations
{
    public partial class Actualizar_Campo_imagenes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Crear una columna temporal para almacenar los datos en el nuevo tipo
            migrationBuilder.AddColumn<byte[]>(
                name: "imagen_temp",
                table: "vehiculos",
                type: "varbinary(max)",
                nullable: true);

            // Copiar los datos existentes de la columna 'imagen' a 'imagen_temp' con conversión explícita
            migrationBuilder.Sql(
                "UPDATE vehiculos SET imagen_temp = CONVERT(varbinary(max), imagen)");

            // Eliminar la columna original
            migrationBuilder.DropColumn(
                name: "imagen",
                table: "vehiculos");

            // Renombrar la columna temporal al nombre original
            migrationBuilder.RenameColumn(
                name: "imagen_temp",
                table: "vehiculos",
                newName: "imagen");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Crear una columna temporal para almacenar los datos en el tipo anterior
            migrationBuilder.AddColumn<string>(
                name: "imagen_temp",
                table: "vehiculos",
                type: "nvarchar(max)",
                nullable: true);

            // Copiar los datos existentes de la columna 'imagen' a 'imagen_temp' con conversión explícita
            migrationBuilder.Sql(
                "UPDATE vehiculos SET imagen_temp = CONVERT(nvarchar(max), imagen)");

            // Eliminar la columna original
            migrationBuilder.DropColumn(
                name: "imagen",
                table: "vehiculos");

            // Renombrar la columna temporal al nombre original
            migrationBuilder.RenameColumn(
                name: "imagen_temp",
                table: "vehiculos",
                newName: "imagen");
        }
    }
}
