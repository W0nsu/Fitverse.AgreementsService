using System;

namespace Fitverse.AgreementsService.Dtos
{
	public class InstallmentDto
	{
		public int Id { get; set; }

		public int AgreementId { get; set; }

		public float Price { get; set; }

		public DateTime StartingDate { get; set; }

		public DateTime EndingDate { get; set; }

		public DateTime DueDate { get; set; }

		public bool IsPaid { get; set; }

		public bool IsChecked { get; set; }
	}
}