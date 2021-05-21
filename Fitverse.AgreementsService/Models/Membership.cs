using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitverse.AgreementsService.Models
{
	public class Membership
	{
		[Key]
		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int MembershipId { get; private set; }

		[Required]
		[MinLength(3)]
		[MaxLength(30)]
		public string Name { get; set; }

		[Required] public int PeriodType { get; set; }

		[Required] public int Duration { get; set; }

		[Required] public int TerminationPeriod { get; set; }

		[Required] public float InstallmentPrice { get; set; }

		[Required] public bool IsDeleted { get; set; }
	}
}