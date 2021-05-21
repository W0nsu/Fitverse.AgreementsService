namespace Fitverse.AgreementsService.Dtos
{
	public class MembershipDto
	{
		public int MembershipId { get; private set; }

		public string Name { get; set; }

		public int PeriodType { get; set; }

		public int Duration { get; set; }

		public int TerminationPeriod { get; set; }

		public float InstallmentPrice { get; set; }
	}
}