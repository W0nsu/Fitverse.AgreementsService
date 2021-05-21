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
	public class EndAgreementHandler : IRequestHandler<EndAgreementCommand, AgreementDto>
	{
		private readonly AgreementsContext _dbContext;
		private DateTime _agreementEndingDate;

		public EndAgreementHandler(AgreementsContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<AgreementDto> Handle(EndAgreementCommand request, CancellationToken cancellationToken)
		{
			var agreementEntity = await _dbContext
				.Agreements
				.SingleOrDefaultAsync(m => m.AgreementId == request.AgreementId,
					cancellationToken);

			var installmentsList = await _dbContext
				.Installments
				.Where(m => m.AgreementId == agreementEntity.AgreementId)
				.OrderBy(m => m.StartingDate)
				.ToListAsync(cancellationToken);

			if (installmentsList is null)
			{
				throw new NullReferenceException(
					$"Installments for agreement [AgreementId: {request.AgreementId}] not found");
			}

			_agreementEndingDate = request.EndDate;

			DeleteInstallments(installmentsList);

			agreementEntity.EndingDate = _agreementEndingDate;

			_ = await _dbContext.SaveChangesAsync(cancellationToken);

			var agreementDto = agreementEntity.Adapt<AgreementDto>();

			return agreementDto;
		}

		private void DeleteInstallments(List<Installment> installmentsList)
		{
			for (var installmentNumber = 0; installmentNumber < installmentsList.Count; installmentNumber++)
			{
				if (installmentsList[installmentNumber].StartingDate > _agreementEndingDate ||
				    installmentsList[installmentNumber].EndingDate < _agreementEndingDate)
					continue;

				installmentsList[installmentNumber].Price =
					CalculatePartialInstallmentPrice(installmentsList[installmentNumber]);

				installmentsList[installmentNumber].EndingDate = _agreementEndingDate;

				installmentsList.RemoveRange(0, installmentNumber + 1);
				break;
			}

			foreach (var installment in installmentsList)
				_dbContext.Remove((object) installment);
		}

		private float CalculatePartialInstallmentPrice(Installment installment)
		{
			var price = installment.Price;

			var daysInFullInstallment = (int) (installment.StartingDate - installment.EndingDate.AddDays(1))
				.TotalDays;
			var daysInPartialInstallment = (int) (installment.StartingDate - _agreementEndingDate.AddDays(1))
				.TotalDays;

			var partialPrice = (float) Math.Round(daysInPartialInstallment * price / daysInFullInstallment, 2);

			return partialPrice;
		}
	}
}