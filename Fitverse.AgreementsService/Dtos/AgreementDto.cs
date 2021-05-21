using System;

namespace Fitverse.AgreementsService.Dtos
{
	public class AgreementDto
	{
		public int AgreementId { get; set; }

		public string Name { get; set; }

		public int MembershipId { get; set; }

		public int MemberId { get; set; }

		public DateTime StartingDate { get; set; }

		public DateTime EndingDate { get; set; }

		public bool IsPaid { get; set; }
	}
}