using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddVoteIdToOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Option_Votes_VoteId",
                table: "Option");

            migrationBuilder.AlterColumn<int>(
                name: "VoteId",
                table: "Option",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Option_Votes_VoteId",
                table: "Option",
                column: "VoteId",
                principalTable: "Votes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Option_Votes_VoteId",
                table: "Option");

            migrationBuilder.AlterColumn<int>(
                name: "VoteId",
                table: "Option",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Option_Votes_VoteId",
                table: "Option",
                column: "VoteId",
                principalTable: "Votes",
                principalColumn: "Id");
        }
    }
}
