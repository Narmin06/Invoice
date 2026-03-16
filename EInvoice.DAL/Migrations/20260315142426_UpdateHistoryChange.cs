using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EInvoice.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHistoryChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceUpdateHistory_Invoices_InvoiceId",
                table: "InvoiceUpdateHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceUpdateHistory",
                table: "InvoiceUpdateHistory");

            migrationBuilder.RenameTable(
                name: "InvoiceUpdateHistory",
                newName: "InvoiceUpdateHistories");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceUpdateHistory_InvoiceId",
                table: "InvoiceUpdateHistories",
                newName: "IX_InvoiceUpdateHistories_InvoiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceUpdateHistories",
                table: "InvoiceUpdateHistories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceUpdateHistories_Invoices_InvoiceId",
                table: "InvoiceUpdateHistories",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceUpdateHistories_Invoices_InvoiceId",
                table: "InvoiceUpdateHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceUpdateHistories",
                table: "InvoiceUpdateHistories");

            migrationBuilder.RenameTable(
                name: "InvoiceUpdateHistories",
                newName: "InvoiceUpdateHistory");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceUpdateHistories_InvoiceId",
                table: "InvoiceUpdateHistory",
                newName: "IX_InvoiceUpdateHistory_InvoiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceUpdateHistory",
                table: "InvoiceUpdateHistory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceUpdateHistory_Invoices_InvoiceId",
                table: "InvoiceUpdateHistory",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
