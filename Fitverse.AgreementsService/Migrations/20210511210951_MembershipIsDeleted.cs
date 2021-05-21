using Microsoft.EntityFrameworkCore.Migrations;

namespace Fitverse.AgreementsService.Migrations
{
	public partial class MembershipIsDeleted : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<bool>(
				"IsDeleted",
				"Memberships",
				"bit",
				nullable: false,
				defaultValue: false);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				"IsDeleted",
				"Memberships");
		}
	}
}