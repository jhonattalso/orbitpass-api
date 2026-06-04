using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OrbitPass.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataTours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DATAS_TOUR",
                columns: new[] { "ID", "DATA_PARTIDA", "DESTINO", "PRECO_BASE" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 10, 15, 8, 0, 0, 0, DateTimeKind.Unspecified), "Órbita Baixa Terrestre", 50000.00m },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 12, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "Estação Lunar Artemis", 250000.00m },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2027, 5, 20, 14, 30, 0, 0, DateTimeKind.Unspecified), "Colônia de Marte", 1500000.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DATAS_TOUR",
                keyColumn: "ID",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "DATAS_TOUR",
                keyColumn: "ID",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "DATAS_TOUR",
                keyColumn: "ID",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));
        }
    }
}
