using System.Collections.Generic;
using Fitverse.AgreementsService.Dtos;
using MediatR;

namespace Fitverse.AgreementsService.Queries
{
	public class GetAllAgreementsByMemberIdQuery : IRequest<List<AgreementDto>>
	{
		public GetAllAgreementsByMemberIdQuery(int memberId)
		{
			MemberId = memberId;
		}

		public int MemberId { get; }
	}
}