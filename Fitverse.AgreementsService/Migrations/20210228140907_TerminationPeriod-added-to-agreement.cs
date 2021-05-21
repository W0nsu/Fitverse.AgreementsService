using Microsoft.EntityFrameworkCore.Migrations;

namespace Fitverse.AgreementsService.Migrations
{
	public partial class TerminationPeriodaddedtoagreement : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				"FK_Installments_Agreements_AgreementId",
				"Installments");

			migrationBuilder.DropIndex(
				"IX_Installments_AgreementId",
				"Installments");

			migrationBuilder.AlterColumn<int>(
				"AgreementId",
				"Installments",
				"int",
				nullable: false,
				defaultValue: 0,
				oldClrType: typeof(int),
				oldType: "int",
				oldNullable: true);

			migrationBuilder.AddColumn<int>(
				"TerminationPeriod",
				"Agreements",
				"int",
				nullable: false,
				defaultValue: 0);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				"TerminationPeriod",
				"Agreements");

			migrationBuilder.AlterColumn<int>(
				"AgreementId",
				"Installments",
				"int",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "int");

			migrationBuilder.CreateIndex(
				"IX_Installments_AgreementId",
				"Installments",
				"AgreementId");

			migrationBuilder.AddForeignKey(
				"FK_Installments_Agreements_AgreementId",
				"Installments",
				"AgreementId",
				"Agreements",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}
	}
}