using System;
using Fitverse.AgreementsService.Dtos;
using MediatR;

namespace Fitverse.AgreementsService.Commands
{
	public class TerminateAgreementCommand : IRequest<AgreementDto>
	{
		public TerminateAgreementCommand(int agreementId, DateTime terminationDate)
		{
			AgreementId = agreementId;
			TerminationDate = terminationDate;
		}

		public DateTime TerminationDate { get; }

		public int AgreementId { get; }
	}
}