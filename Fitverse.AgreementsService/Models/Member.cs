using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitverse.AgreementsService.Models
{
	public class Member
	{
		[Key]
		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required] public int MemberId { get; set; }

		[Required] public string Name { get; set; }

		[Required] public string SurName { get; set; }
	}
}