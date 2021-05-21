using Fitverse.AgreementsService.Dtos;
using MediatR;

namespace Fitverse.AgreementsService.Queries
{
	public class GetAgreementByIdQuery : IRequest<AgreementDto>
	{
		public GetAgreementByIdQuery(int agreementId)
		{
			AgreementId = agreementId;
		}

		public int AgreementId { get; }
	}
}