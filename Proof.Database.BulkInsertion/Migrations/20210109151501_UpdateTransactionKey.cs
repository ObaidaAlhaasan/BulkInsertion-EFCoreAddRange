using Microsoft.EntityFrameworkCore.Migrations;

namespace Proof.Database.BulkInsertion.Migrations
{
    public partial class UpdateTransactionKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Transactions_LoanId",
                table: "Transactions",
                column: "LoanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transactions_LoanId",
                table: "Transactions");
        }
    }
}
