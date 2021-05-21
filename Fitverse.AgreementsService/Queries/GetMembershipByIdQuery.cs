using Fitverse.AgreementsService.Dtos;
using MediatR;

namespace Fitverse.AgreementsService.Queries
{
	public class GetMembershipByIdQuery : IRequest<MembershipDto>
	{
		public GetMembershipByIdQuery(int id)
		{
			Id = id;
		}

		public int Id { get; }
	}
}