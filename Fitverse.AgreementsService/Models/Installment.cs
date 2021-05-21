using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitverse.AgreementsService.Models
{
	public class Installment
	{
		[Key]
		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required] public int AgreementId { get; set; }

		[Required] public float Price { get; set; }

		[Required] [Column(TypeName = "Date")] public DateTime StartingDate { get; set; }

		[Required] [Column(TypeName = "Date")] public DateTime EndingDate { get; set; }

		[Required] [Column(TypeName = "Date")] public DateTime DueDate { get; set; }

		[Required] public bool IsPaid { get; set; }

		[Required] public bool IsChecked { get; set; }
	}
}