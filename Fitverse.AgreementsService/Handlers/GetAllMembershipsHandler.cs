using System.Collections.Generic;
using System.Linq;
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
	public class GetAllMembershipsHandler : IRequestHandler<GetAllMembershipsQuery, List<MembershipDto>>
	{
		private readonly AgreementsContext _dbContext;

		public GetAllMembershipsHandler(AgreementsContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<MembershipDto>> Handle(GetAllMembershipsQuery request,
			CancellationToken cancellationToken)
		{
			var membershipsList = await _dbContext
				.Memberships
				.Where(x => !x.IsDeleted)
				.ToListAsync(cancellationToken);

			return membershipsList.Select(member => member.Adapt<MembershipDto>()).ToList();
		}
	}
}