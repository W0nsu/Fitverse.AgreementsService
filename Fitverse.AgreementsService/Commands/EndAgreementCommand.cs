using System;
using Fitverse.AgreementsService.Dtos;
using MediatR;

namespace Fitverse.AgreementsService.Commands
{
	public class EndAgreementCommand : IRequest<AgreementDto>
	{
		public EndAgreementCommand(int agreementId, DateTime endDate)
		{
			AgreementId = agreementId;
			EndDate = endDate;
		}

		public DateTime EndDate { get; }

		public int AgreementId { get; }
	}
}