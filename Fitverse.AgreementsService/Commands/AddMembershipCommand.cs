using Fitverse.AgreementsService.Dtos;
using MediatR;

namespace Fitverse.AgreementsService.Commands
{
	public class AddMembershipCommand : IRequest<MembershipDto>
	{
		public AddMembershipCommand(MembershipDto membership)
		{
			Membership = membership;
		}

		public MembershipDto Membership { get; }
	}
}