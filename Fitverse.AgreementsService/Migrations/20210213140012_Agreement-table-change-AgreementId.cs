using Microsoft.EntityFrameworkCore.Migrations;

namespace Fitverse.AgreementsService.Migrations
{
	public partial class AgreementtablechangeAgreementId : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				"FK_Agreements_Members_MemberId",
				"Agreements");

			migrationBuilder.DropForeignKey(
				"FK_Agreements_Memberships_MembershipId",
				"Agreements");

			migrationBuilder.DropIndex(
				"IX_Agreements_MemberId",
				"Agreements");

			migrationBuilder.DropIndex(
				"IX_Agreements_MembershipId",
				"Agreements");

			migrationBuilder.AlterColumn<int>(
				"MembershipId",
				"Agreements",
				"int",
				nullable: false,
				defaultValue: 0,
				oldClrType: typeof(int),
				oldType: "int",
				oldNullable: true);

			migrationBuilder.AlterColumn<int>(
				"MemberId",
				"Agreements",
				"int",
				nullable: false,
				defaultValue: 0,
				oldClrType: typeof(int),
				oldType: "int",
				oldNullable: true);

			migrationBuilder.AddColumn<int>(
				"AgreementId",
				"Agreements",
				"int",
				nullable: false,
				defaultValue: 0);

			migrationBuilder.CreateIndex(
				"IX_Agreements_AgreementId",
				"Agreements",
				"AgreementId",
				unique: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				"IX_Agreements_AgreementId",
				"Agreements");

			migrationBuilder.DropColumn(
				"AgreementId",
				"Agreements");

			migrationBuilder.AlterColumn<int>(
				"MembershipId",
				"Agreements",
				"int",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "int");

			migrationBuilder.AlterColumn<int>(
				"MemberId",
				"Agreements",
				"int",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "int");

			migrationBuilder.CreateIndex(
				"IX_Agreements_MemberId",
				"Agreements",
				"MemberId");

			migrationBuilder.CreateIndex(
				"IX_Agreements_MembershipId",
				"Agreements",
				"MembershipId");

			migrationBuilder.AddForeignKey(
				"FK_Agreements_Members_MemberId",
				"Agreements",
				"MemberId",
				"Members",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				"FK_Agreements_Memberships_MembershipId",
				"Agreements",
				"MembershipId",
				"Memberships",
				principalColumn: "MembershipId",
				onDelete: ReferentialAction.Restrict);
		}
	}
}