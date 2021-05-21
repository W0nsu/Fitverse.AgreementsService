using Fitverse.AgreementsService.Models;
using Microsoft.EntityFrameworkCore;

namespace Fitverse.AgreementsService.Data
{
	public class AgreementsContext : DbContext
	{
		public AgreementsContext(DbContextOptions<AgreementsContext> options) : base(options)
		{
		}

		public DbSet<Membership> Memberships { get; set; }

		public DbSet<Agreement> Agreements { get; set; }

		public DbSet<Installment> Installments { get; set; }

		public DbSet<Member> Members { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<Member>()
				.HasIndex(u => u.MemberId)
				.IsUnique();

			builder.Entity<Agreement>()
				.HasIndex(u => u.AgreementId)
				.IsUnique();
		}
	}
}