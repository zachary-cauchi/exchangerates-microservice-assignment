using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exchange.API.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountBalances : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "account_balance_hilo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "account_balance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(19,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_balance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_account_balance_currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_account_balance_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_account_balance_CurrencyId",
                table: "account_balance",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_account_balance_UserId",
                table: "account_balance",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_balance");

            migrationBuilder.DropSequence(
                name: "account_balance_hilo");
        }
    }
}
