using System;
using System.Threading;
using System.Threading.Tasks;
using Fitverse.AgreementsService.Data;
using Fitverse.AgreementsService.Dtos;
using Fitverse.AgreementsService.Queries;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fitverse.AgreementsService.Handlers
{
	public class GetMembershipByIdHandler : IRequestHandler<GetMembershipByIdQuery, MembershipDto>
	{
		private readonly AgreementsContext _dbContext;

		public GetMembershipByIdHandler(AgreementsContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<MembershipDto> Handle(GetMembershipByIdQuery request, CancellationToken cancellationToken)
		{
			var membershipEntity = await _dbContext
				.Memberships
				.SingleOrDefaultAsync(m => m.MembershipId == request.Id && !m.IsDeleted, cancellationToken);

			if (membershipEntity is null)
				throw new NullReferenceException($"Membership [MembershipId: {request.Id} not found]");

			var membershipDto = membershipEntity.Adapt<MembershipDto>();

			return membershipDto;
		}
	}
}