using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fitverse.AgreementsService.Commands;
using Fitverse.AgreementsService.Data;
using Fitverse.AgreementsService.Dtos;
using Fitverse.AgreementsService.Models;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fitverse.AgreementsService.Handlers
{
	public class TerminateAgreementHandler : IRequestHandler<TerminateAgreementCommand, AgreementDto>
	{
		private readonly AgreementsContext _dbContext;

		public TerminateAgreementHandler(AgreementsContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<AgreementDto> Handle(TerminateAgreementCommand request, CancellationToken cancellationToken)
		{
			var agreementEntity = await _dbContext.Agreements
				.SingleOrDefaultAsync(m => m.AgreementId == request.AgreementId,
					cancellationToken);

			var installmentsList = await _dbContext.Installments
				.Where(m => m.AgreementId == agreementEntity.AgreementId)
				.OrderBy(m => m.StartingDate)
				.ToListAsync(cancellationToken);

			if (installmentsList is null)
			{
				throw new NullReferenceException(
					$"Installments for agreement [AgreementId: {request.AgreementId}] not found");
			}

			var terminationDate = request.TerminationDate;
			var terminationPeriod = agreementEntity.TerminationPeriod;

			DeleteInstallments(installmentsList, terminationDate, terminationPeriod);

			agreementEntity.EndingDate = installmentsList[0]
				.StartingDate
				.AddDays(-1);

			_ = await _dbContext.SaveChangesAsync(cancellationToken);

			var agreementDto = agreementEntity.Adapt<AgreementDto>();

			return agreementDto;
		}

		private void DeleteInstallments(List<Installment> installmentsList, DateTime terminationDate,
			int terminationPeriod)
		{
			for (var installmentNumber = 0; installmentNumber < installmentsList.Count; installmentNumber++)
			{
				if (installmentsList[installmentNumber].StartingDate > terminationDate ||
				    installmentsList[installmentNumber].EndingDate < terminationDate)
					continue;

				installmentsList.RemoveRange(0, installmentNumber + terminationPeriod + 1);
				break;
			}

			foreach (var installment in installmentsList)
				_dbContext.Remove((object) installment);
		}
	}
}