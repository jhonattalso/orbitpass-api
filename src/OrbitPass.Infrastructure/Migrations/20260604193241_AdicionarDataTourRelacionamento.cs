using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrbitPass.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarDataTourRelacionamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DATAS_TOUR",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    DESTINO = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    DATA_PARTIDA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    PRECO_BASE = table.Column<decimal>(type: "NUMBER(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DATAS_TOUR", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_INGRESSOS_DATA_TOUR_ID",
                table: "INGRESSOS",
                column: "DATA_TOUR_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_INGRESSOS_DATAS_TOUR_DATA_TOUR_ID",
                table: "INGRESSOS",
                column: "DATA_TOUR_ID",
                principalTable: "DATAS_TOUR",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_INGRESSOS_DATAS_TOUR_DATA_TOUR_ID",
                table: "INGRESSOS");

            migrationBuilder.DropTable(
                name: "DATAS_TOUR");

            migrationBuilder.DropIndex(
                name: "IX_INGRESSOS_DATA_TOUR_ID",
                table: "INGRESSOS");
        }
    }
}
