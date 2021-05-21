using System.Linq;
using Fitverse.AgreementsService.Commands;
using Fitverse.AgreementsService.Data;
using FluentValidation;

namespace Fitverse.AgreementsService.Validators
{
	public class DeleteMembershipCommandValidator : AbstractValidator<DeleteMembershipCommand>
	{
		public DeleteMembershipCommandValidator(AgreementsContext dbContext)
		{
			RuleFor(x => x.MembershipId)
				.GreaterThan(0);

			RuleFor(x => x.MembershipId)
				.Must(id => dbContext.Memberships.Any(m => m.MembershipId == id))
				.WithMessage(x => $"Membership [MembershipId: {x.MembershipId}] not found.");
		}
	}
}