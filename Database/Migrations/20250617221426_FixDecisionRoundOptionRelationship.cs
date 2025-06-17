using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class FixDecisionRoundOptionRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decision_RoundOptions_RoundOptionOptionId_RoundOptionRoundId",
                table: "Decision");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Decision",
                table: "Decision");

            migrationBuilder.RenameTable(
                name: "Decision",
                newName: "Decisions");

            migrationBuilder.RenameColumn(
                name: "RoundOptionRoundId",
                table: "Decisions",
                newName: "RoundId");

            migrationBuilder.RenameColumn(
                name: "RoundOptionOptionId",
                table: "Decisions",
                newName: "OptionId");

            migrationBuilder.RenameIndex(
                name: "IX_Decision_RoundOptionOptionId_RoundOptionRoundId",
                table: "Decisions",
                newName: "IX_Decisions_OptionId_RoundId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Decisions",
                table: "Decisions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Decisions_RoundOptions_OptionId_RoundId",
                table: "Decisions",
                columns: new[] { "OptionId", "RoundId" },
                principalTable: "RoundOptions",
                principalColumns: new[] { "OptionId", "RoundId" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decisions_RoundOptions_OptionId_RoundId",
                table: "Decisions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Decisions",
                table: "Decisions");

            migrationBuilder.RenameTable(
                name: "Decisions",
                newName: "Decision");

            migrationBuilder.RenameColumn(
                name: "RoundId",
                table: "Decision",
                newName: "RoundOptionRoundId");

            migrationBuilder.RenameColumn(
                name: "OptionId",
                table: "Decision",
                newName: "RoundOptionOptionId");

            migrationBuilder.RenameIndex(
                name: "IX_Decisions_OptionId_RoundId",
                table: "Decision",
                newName: "IX_Decision_RoundOptionOptionId_RoundOptionRoundId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Decision",
                table: "Decision",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Decision_RoundOptions_RoundOptionOptionId_RoundOptionRoundId",
                table: "Decision",
                columns: new[] { "RoundOptionOptionId", "RoundOptionRoundId" },
                principalTable: "RoundOptions",
                principalColumns: new[] { "OptionId", "RoundId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
