using System;
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
	public class
		GetAllInstallmentsByAgreementIdHandler : IRequestHandler<GetAllInstallmentsByAgreementIdQuery,
			List<InstallmentDto>>
	{
		private readonly AgreementsContext _dbContext;

		public GetAllInstallmentsByAgreementIdHandler(AgreementsContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<InstallmentDto>> Handle(GetAllInstallmentsByAgreementIdQuery request,
			CancellationToken cancellationToken)
		{
			var installmentsList = await _dbContext
				.Installments
				.Where(m => m.AgreementId == request.AgreementId)
				.OrderBy(m => m.StartingDate)
				.ToListAsync(cancellationToken);

			if (installmentsList is null)
			{
				throw new NullReferenceException(
					$"Installments for agreement [agreementId: {request.AgreementId}] not found");
			}

			return installmentsList.Select(installment => installment.Adapt<InstallmentDto>()).ToList();
		}
	}
}