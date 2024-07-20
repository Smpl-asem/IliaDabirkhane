using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IliaDabirkhane.Migrations
{
    /// <inheritdoc />
    public partial class newr1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attecheds_tbl_Messages_tbl_MessagesId",
                table: "Attecheds_tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_Recivers_tbl_Messages_tbl_MessagesId",
                table: "Recivers_tbl");

            migrationBuilder.DropIndex(
                name: "IX_Recivers_tbl_MessagesId",
                table: "Recivers_tbl");

            migrationBuilder.DropIndex(
                name: "IX_Attecheds_tbl_MessagesId",
                table: "Attecheds_tbl");

            migrationBuilder.DropColumn(
                name: "MessagesId",
                table: "Recivers_tbl");

            migrationBuilder.DropColumn(
                name: "MessagesId",
                table: "Attecheds_tbl");

            migrationBuilder.CreateIndex(
                name: "IX_Recivers_tbl_MessageId",
                table: "Recivers_tbl",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Attecheds_tbl_MessageId",
                table: "Attecheds_tbl",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attecheds_tbl_Messages_tbl_MessageId",
                table: "Attecheds_tbl",
                column: "MessageId",
                principalTable: "Messages_tbl",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Recivers_tbl_Messages_tbl_MessageId",
                table: "Recivers_tbl",
                column: "MessageId",
                principalTable: "Messages_tbl",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attecheds_tbl_Messages_tbl_MessageId",
                table: "Attecheds_tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_Recivers_tbl_Messages_tbl_MessageId",
                table: "Recivers_tbl");

            migrationBuilder.DropIndex(
                name: "IX_Recivers_tbl_MessageId",
                table: "Recivers_tbl");

            migrationBuilder.DropIndex(
                name: "IX_Attecheds_tbl_MessageId",
                table: "Attecheds_tbl");

            migrationBuilder.AddColumn<int>(
                name: "MessagesId",
                table: "Recivers_tbl",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MessagesId",
                table: "Attecheds_tbl",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recivers_tbl_MessagesId",
                table: "Recivers_tbl",
                column: "MessagesId");

            migrationBuilder.CreateIndex(
                name: "IX_Attecheds_tbl_MessagesId",
                table: "Attecheds_tbl",
                column: "MessagesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attecheds_tbl_Messages_tbl_MessagesId",
                table: "Attecheds_tbl",
                column: "MessagesId",
                principalTable: "Messages_tbl",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Recivers_tbl_Messages_tbl_MessagesId",
                table: "Recivers_tbl",
                column: "MessagesId",
                principalTable: "Messages_tbl",
                principalColumn: "Id");
        }
    }
}
