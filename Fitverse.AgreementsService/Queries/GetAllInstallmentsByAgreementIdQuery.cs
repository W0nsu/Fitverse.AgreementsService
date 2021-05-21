using System.Collections.Generic;
using Fitverse.AgreementsService.Dtos;
using MediatR;

namespace Fitverse.AgreementsService.Queries
{
	public class GetAllInstallmentsByAgreementIdQuery : IRequest<List<InstallmentDto>>
	{
		public GetAllInstallmentsByAgreementIdQuery(int agreementId)
		{
			AgreementId = agreementId;
		}

		public int AgreementId { get; }
	}
}