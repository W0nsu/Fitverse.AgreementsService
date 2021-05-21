using System.Collections.Generic;
using Fitverse.AgreementsService.Dtos;
using MediatR;

namespace Fitverse.AgreementsService.Queries
{
	public class GetAllMembershipsQuery : IRequest<List<MembershipDto>>
	{
	}
}