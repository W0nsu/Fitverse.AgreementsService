namespace Fitverse.AgreementsService.Dtos
{
	public class OverdueInstallmentDto
	{
		public string MemberFirstName { get; set; }

		public string MemberSurname { get; set; }

		public string MembershipName { get; set; }

		public InstallmentDto InstallmentDetails { get; set; }
	}
}