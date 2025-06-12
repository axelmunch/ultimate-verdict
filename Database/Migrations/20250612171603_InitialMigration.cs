using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Votes_Type",
                table: "Votes");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Votes_VictoryCondition",
                table: "Votes");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Votes_Visibility",
                table: "Votes");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Results_Res",
                table: "Results");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Votes_Type",
                table: "Votes",
                sql: "\"Type\" IN ('plural', 'ranked', 'weighted', 'elo')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Votes_VictoryCondition",
                table: "Votes",
                sql: "\"VictoryCondition\" IN ('none', 'majority', 'absolute majority', '2/3 majority', 'last man standing')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Votes_Visibility",
                table: "Votes",
                sql: "\"Visibility\" IN ('public', 'private')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Results_Res",
                table: "Results",
                sql: "\"Res\" IN ('winner', 'draw', 'inconclusive')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Votes_Type",
                table: "Votes");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Votes_VictoryCondition",
                table: "Votes");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Votes_Visibility",
                table: "Votes");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Results_Res",
                table: "Results");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Votes_Type",
                table: "Votes",
                sql: "type IN ('plural', 'ranked', 'weighted', 'elo')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Votes_VictoryCondition",
                table: "Votes",
                sql: "victory_condition IN ('none', 'majority', 'absolute majority', '2/3 majority', 'last man standing')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Votes_Visibility",
                table: "Votes",
                sql: "visibility IN ('public', 'private')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Results_Res",
                table: "Results",
                sql: "res IN ('winner', 'draw', 'inconclusive')");
        }
    }
}
