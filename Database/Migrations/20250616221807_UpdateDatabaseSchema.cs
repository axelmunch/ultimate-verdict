using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Votes_VoteId",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "LiveResults",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "ResultId",
                table: "Votes");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Votes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int[]>(
                name: "WinnersByRounds",
                table: "Votes",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);

            migrationBuilder.AlterColumn<int>(
                name: "VoteId",
                table: "Rounds",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "VoteId",
                table: "Option",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoundOptionOptionId",
                table: "Decision",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoundOptionRoundId",
                table: "Decision",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Option_VoteId",
                table: "Option",
                column: "VoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Decision_RoundOptionOptionId_RoundOptionRoundId",
                table: "Decision",
                columns: new[] { "RoundOptionOptionId", "RoundOptionRoundId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Decision_RoundOptions_RoundOptionOptionId_RoundOptionRoundId",
                table: "Decision",
                columns: new[] { "RoundOptionOptionId", "RoundOptionRoundId" },
                principalTable: "RoundOptions",
                principalColumns: new[] { "OptionId", "RoundId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Option_Votes_VoteId",
                table: "Option",
                column: "VoteId",
                principalTable: "Votes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Votes_VoteId",
                table: "Rounds",
                column: "VoteId",
                principalTable: "Votes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decision_RoundOptions_RoundOptionOptionId_RoundOptionRoundId",
                table: "Decision");

            migrationBuilder.DropForeignKey(
                name: "FK_Option_Votes_VoteId",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Votes_VoteId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Option_VoteId",
                table: "Option");

            migrationBuilder.DropIndex(
                name: "IX_Decision_RoundOptionOptionId_RoundOptionRoundId",
                table: "Decision");

            migrationBuilder.DropColumn(
                name: "WinnersByRounds",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "VoteId",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "RoundOptionOptionId",
                table: "Decision");

            migrationBuilder.DropColumn(
                name: "RoundOptionRoundId",
                table: "Decision");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Votes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<bool>(
                name: "LiveResults",
                table: "Votes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ResultId",
                table: "Votes",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VoteId",
                table: "Rounds",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Votes_VoteId",
                table: "Rounds",
                column: "VoteId",
                principalTable: "Votes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
