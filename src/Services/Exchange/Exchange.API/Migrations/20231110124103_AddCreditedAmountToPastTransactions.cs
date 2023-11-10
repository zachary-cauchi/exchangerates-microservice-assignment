using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exchange.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCreditedAmountToPastTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CreditedAmount",
                table: "past_transaction",
                type: "decimal(19,4)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreditedAmount",
                table: "past_transaction");
        }
    }
}
