using Fitverse.AgreementsService.Dtos;
using MediatR;

namespace Fitverse.AgreementsService.Commands
{
	public class DeleteMembershipCommand : IRequest<MembershipDto>
	{
		public DeleteMembershipCommand(int membershipId)
		{
			MembershipId = membershipId;
		}

		public int MembershipId { get; }
	}
}