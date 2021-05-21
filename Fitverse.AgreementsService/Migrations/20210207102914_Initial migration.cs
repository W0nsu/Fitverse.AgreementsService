using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fitverse.AgreementsService.Migrations
{
	public partial class Initialmigration : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				"Members",
				table => new
				{
					Id = table.Column<int>("int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					MemberId = table.Column<int>("int", nullable: false),
					Name = table.Column<string>("nvarchar(max)", nullable: false),
					SurName = table.Column<string>("nvarchar(max)", nullable: false)
				},
				constraints: table => { table.PrimaryKey("PK_Members", x => x.Id); });

			migrationBuilder.CreateTable(
				"Memberships",
				table => new
				{
					MembershipId = table.Column<int>("int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>("nvarchar(30)", maxLength: 30, nullable: false),
					PeriodType = table.Column<int>("int", nullable: false),
					Duration = table.Column<int>("int", nullable: false),
					TerminationPeriod = table.Column<int>("int", nullable: false),
					InstallmentPrice = table.Column<float>("real", nullable: false)
				},
				constraints: table => { table.PrimaryKey("PK_Memberships", x => x.MembershipId); });

			migrationBuilder.CreateTable(
				"Agreements",
				table => new
				{
					Id = table.Column<int>("int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>("nvarchar(30)", maxLength: 30, nullable: false),
					MembershipId = table.Column<int>("int", nullable: true),
					MemberId = table.Column<int>("int", nullable: true),
					StartingDate = table.Column<DateTime>("Date", nullable: false),
					EndingDate = table.Column<DateTime>("Date", nullable: false),
					IsPaid = table.Column<bool>("bit", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Agreements", x => x.Id);
					table.ForeignKey(
						"FK_Agreements_Members_MemberId",
						x => x.MemberId,
						"Members",
						"Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						"FK_Agreements_Memberships_MembershipId",
						x => x.MembershipId,
						"Memberships",
						"MembershipId",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				"Installments",
				table => new
				{
					Id = table.Column<int>("int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					AgreementId = table.Column<int>("int", nullable: true),
					Price = table.Column<float>("real", nullable: false),
					StartingDate = table.Column<DateTime>("Date", nullable: false),
					EndingDate = table.Column<DateTime>("Date", nullable: false),
					DueDate = table.Column<DateTime>("Date", nullable: false),
					IsPaid = table.Column<bool>("bit", nullable: false),
					IsChecked = table.Column<bool>("bit", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Installments", x => x.Id);
					table.ForeignKey(
						"FK_Installments_Agreements_AgreementId",
						x => x.AgreementId,
						"Agreements",
						"Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				"IX_Agreements_MemberId",
				"Agreements",
				"MemberId");

			migrationBuilder.CreateIndex(
				"IX_Agreements_MembershipId",
				"Agreements",
				"MembershipId");

			migrationBuilder.CreateIndex(
				"IX_Installments_AgreementId",
				"Installments",
				"AgreementId");

			migrationBuilder.CreateIndex(
				"IX_Members_MemberId",
				"Members",
				"MemberId",
				unique: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				"Installments");

			migrationBuilder.DropTable(
				"Agreements");

			migrationBuilder.DropTable(
				"Members");

			migrationBuilder.DropTable(
				"Memberships");
		}
	}
}