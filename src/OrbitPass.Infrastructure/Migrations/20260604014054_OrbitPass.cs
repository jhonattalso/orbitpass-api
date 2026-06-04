using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrbitPass.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OrbitPass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "INGRESSOS",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    USUARIO_ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    DATA_TOUR_ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    CODIGO_UNICO = table.Column<string>(type: "NVARCHAR2(40)", maxLength: 40, nullable: false),
                    STATUS = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DATA_COMPRA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    VALOR_PAGO = table.Column<decimal>(type: "NUMBER(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INGRESSOS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PAGAMENTOS",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    INGRESSO_ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    METODO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    STATUS = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DATA_PAGAMENTO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    VALOR = table.Column<decimal>(type: "NUMBER(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PAGAMENTOS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PAGAMENTOS_INGRESSOS_INGRESSO_ID",
                        column: x => x.INGRESSO_ID,
                        principalTable: "INGRESSOS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_INGRESSOS_CODIGO_UNICO",
                table: "INGRESSOS",
                column: "CODIGO_UNICO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PAGAMENTOS_INGRESSO_ID",
                table: "PAGAMENTOS",
                column: "INGRESSO_ID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PAGAMENTOS");

            migrationBuilder.DropTable(
                name: "INGRESSOS");
        }
    }
}
