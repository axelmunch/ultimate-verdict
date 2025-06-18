using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class FixModelRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Votes_VoteId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_VoteId",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "VoteId",
                table: "Rounds");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_idVote",
                table: "Rounds",
                column: "idVote");

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Votes_idVote",
                table: "Rounds",
                column: "idVote",
                principalTable: "Votes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Votes_idVote",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_idVote",
                table: "Rounds");

            migrationBuilder.AddColumn<int>(
                name: "VoteId",
                table: "Rounds",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_VoteId",
                table: "Rounds",
                column: "VoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Votes_VoteId",
                table: "Rounds",
                column: "VoteId",
                principalTable: "Votes",
                principalColumn: "Id");
        }
    }
}
