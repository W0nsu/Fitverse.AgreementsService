using Fitverse.AgreementsService.Dtos;
using Fitverse.AgreementsService.Models;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Fitverse.AgreementsService.Commands
{
	public class EditMembershipCommand : IRequest<MembershipDto>
	{
		public EditMembershipCommand(int id, JsonPatchDocument<Membership> membership)
		{
			Id = id;
			Membership = membership;
		}

		public int Id { get; }
		public JsonPatchDocument<Membership> Membership { get; }
	}
}