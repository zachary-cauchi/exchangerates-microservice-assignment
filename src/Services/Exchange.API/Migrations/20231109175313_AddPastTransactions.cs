using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exchange.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPastTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "past_transaction_hilo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "past_transaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FromAccountBalanceId = table.Column<int>(type: "int", nullable: false),
                    ToAccountBalanceId = table.Column<int>(type: "int", nullable: false),
                    DebitedAmount = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    TimeEffected = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FromCurrencyId = table.Column<int>(type: "int", nullable: false),
                    ToCurrencyId = table.Column<int>(type: "int", nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "decimal(19,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_past_transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_past_transaction_account_balance_FromAccountBalanceId",
                        column: x => x.FromAccountBalanceId,
                        principalTable: "account_balance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_past_transaction_account_balance_ToAccountBalanceId",
                        column: x => x.ToAccountBalanceId,
                        principalTable: "account_balance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_past_transaction_currency_FromCurrencyId",
                        column: x => x.FromCurrencyId,
                        principalTable: "currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_past_transaction_currency_ToCurrencyId",
                        column: x => x.ToCurrencyId,
                        principalTable: "currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_past_transaction_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_past_transaction_FromAccountBalanceId",
                table: "past_transaction",
                column: "FromAccountBalanceId");

            migrationBuilder.CreateIndex(
                name: "IX_past_transaction_FromCurrencyId",
                table: "past_transaction",
                column: "FromCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_past_transaction_ToAccountBalanceId",
                table: "past_transaction",
                column: "ToAccountBalanceId");

            migrationBuilder.CreateIndex(
                name: "IX_past_transaction_ToCurrencyId",
                table: "past_transaction",
                column: "ToCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_past_transaction_UserId",
                table: "past_transaction",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "past_transaction");

            migrationBuilder.DropSequence(
                name: "past_transaction_hilo");
        }
    }
}
